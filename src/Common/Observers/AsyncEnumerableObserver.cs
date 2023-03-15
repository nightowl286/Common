using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TNO.Common.Disposable;

namespace TNO.Common.Observers;

/// <summary>
/// Provides an asynchronous wrapper around <see cref="IObserver{T}"/>
/// that can be used to receive multiple values.
/// </summary>
/// <typeparam name="T">The type that provides notification information.</typeparam>
public class AsyncEnumerableObserver<T> : DisposableBase, IObserver<T>
{
   #region Fields
   private readonly Queue<T> _values;
   private readonly SemaphoreSlim _completionHandler;
   private readonly TimeSpan _waitTimeout = TimeSpan.FromMilliseconds(5);
   private Exception? _error;
   #endregion

   /// <summary>Creates a new instance of the <see cref="AsyncEnumerableObserver{T}"/>.</summary>
   public AsyncEnumerableObserver()
   {
      _completionHandler = new SemaphoreSlim(0);
      _values = new Queue<T>();
   }

   #region Methods
   /// <inheritdoc/>
   public void OnCompleted() => _completionHandler.Release();

   /// <inheritdoc/>
   public void OnError(Exception error)
   {
      _error = error;

      _completionHandler.Release();
   }

   /// <inheritdoc/>
   public void OnNext(T value) => _values.Enqueue(value);

   /// <summary>
   /// Get an <see cref="IAsyncEnumerable{T}"/> that will asynchronously
   /// iterate over the received values.
   /// </summary>
   /// <param name="cancellationToken">A token that can be used to cancel this operation.</param>
   /// <returns>A asynchronous enumeration over all the received values.</returns>
   public async IAsyncEnumerable<T> GetAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
   {
      ThrowIfDisposed(nameof(AsyncEnumerableObserver<T>));

      try
      {
         while (true)
         {
            while (_values.Count > 0)
            {
               if (cancellationToken.IsCancellationRequested)
               {
                  yield break;
               }

               yield return _values.Dequeue();
            }

            if (await _completionHandler.WaitAsync(_waitTimeout))
               break;

            if (cancellationToken.IsCancellationRequested)
            {
               yield break;
            }
         }
         if (_error is not null)
            throw _error;
      }
      finally
      {
         Dispose();
      }
   }

   /// <inheritdoc/>
   protected override void DisposeManaged()
   {
      _values.Clear();
      _completionHandler.Dispose();
   }
   #endregion
}
