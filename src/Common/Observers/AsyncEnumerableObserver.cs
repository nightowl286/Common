using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace TNO.Common.Observers
{
   /// <summary>
   /// Provides an asynchronous wrapper around <see cref="IObserver{T}"/>
   /// that can be used to receive multiple values.
   /// </summary>
   /// <typeparam name="T">The type that provides notification information.</typeparam>
   public class AsyncEnumerableObserver<T> : IObserver<T>
   {
      #region Fields
      private T _latestValue;
      private bool _yieldValue;
      private SemaphoreSlim _canWriteHandle;
      private SemaphoreSlim _canReadHandle;
      private Exception? _error;
      #endregion

      /// <summary>Creates a new instance of the <see cref="AsyncEnumerableObserver{T}"/>.</summary>
      public AsyncEnumerableObserver()
      {
         _latestValue = default!;

         // Todo(Anyone): Look into maybe using a Read/Write lock for this;
         _canWriteHandle = new SemaphoreSlim(1);
         _canReadHandle = new SemaphoreSlim(0);
      }

      #region Methods
      /// <inheritdoc/>
      public void OnCompleted() => _canReadHandle.Release();

      /// <inheritdoc/>
      public void OnError(Exception error)
      {
         _error = error;

         _canReadHandle.Release();
      }

      /// <inheritdoc/>
      public void OnNext(T value)
      {
         _canWriteHandle.Wait();

         _latestValue = value;
         _yieldValue = true;

         _canReadHandle.Release();
      }

      /// <summary>
      /// Get an <see cref="IAsyncEnumerable{T}"/> that will asynchronously
      /// iterate over the received values.
      /// </summary>
      /// <param name="cancellationToken">A token that can be used to cancel this operation.</param>
      /// <returns>A asynchronous enumeration over all the received values.</returns>
      public async IAsyncEnumerable<T> GetAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
      {
         while (true)
         {
            await _canReadHandle.WaitAsync(cancellationToken);

            if (_yieldValue)
            {
               _yieldValue = false;
               yield return _latestValue;
            }
            else
               break;

            _canWriteHandle.Release();
         }

         _canWriteHandle.Dispose();
         _canReadHandle.Dispose();

         if (_error != null)
            throw _error;
      }
      #endregion
   }
}
