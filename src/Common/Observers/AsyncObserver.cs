using System;
using System.Threading.Tasks;

namespace TNO.Common.Observers
{
   /// <summary>
   /// Provided an asynchronous wrapper around an <see cref="IObserver{T}"/>
   /// that can be used to obtain the latest value.
   /// </summary>
   /// <typeparam name="T">The type that provides notification information.</typeparam>
   public class AsyncObserver<T> : IObserver<T>
   {
      #region Fields
      private bool _hasValue;
      private T _latestResult;
      private TaskCompletionSource<T> _completionSource;
      #endregion

      #region Properties
      /// <summary>The task that can be used to await until this observer <see cref="IObserver{T}.OnCompleted"/> fires.</summary>
      public Task<T> Task => _completionSource.Task;
      #endregion

      /// <summary>Creates an instance of the <see cref="AsyncObserver{T}"/>.</summary>
      public AsyncObserver()
      {
         _completionSource = new TaskCompletionSource<T>();
         _latestResult = default!;
      }

      #region Methods
      /// <inheritdoc/>
      public void OnCompleted()
      {
         if (_hasValue)
         {
            _completionSource.SetResult(_latestResult);
         }
      }

      /// <inheritdoc/>
      public void OnError(Exception error) => _completionSource.SetException(error);

      /// <inheritdoc/>
      public void OnNext(T value)
      {
         _latestResult = value;
         _hasValue = true;
      }
      #endregion
   }
}
