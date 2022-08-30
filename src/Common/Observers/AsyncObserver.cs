using System;
using System.Threading.Tasks;

namespace TNO.Common.Observers
{
   public class AsyncObserver<T> : IObserver<T>
   {
      #region Fields
      private T _latestResult;
      private TaskCompletionSource<T> _completionSource;
      #endregion

      #region Properties
      public Task<T> Task => _completionSource.Task;
      #endregion
      public AsyncObserver()
      {
         _completionSource = new TaskCompletionSource<T>();
         _latestResult = default!;
      }

      #region Methods
      public void OnCompleted() => _completionSource.SetResult(_latestResult);
      public void OnError(Exception error) => _completionSource.SetException(error);
      public void OnNext(T value)
      {
         _latestResult = value;
      }
      #endregion
   }
}
