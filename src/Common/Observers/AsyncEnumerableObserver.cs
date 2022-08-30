using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace TNO.Common.Observers
{
   public class AsyncEnumerableObserver<T> : IObserver<T>
   {
      #region Fields
      private T _latestValue;
      private bool _yieldValue;
      private SemaphoreSlim _canWriteHandle;
      private SemaphoreSlim _canReadHandle;
      private Exception? _error;
      #endregion
      public AsyncEnumerableObserver()
      {
         _latestValue = default!;
         _canWriteHandle = new SemaphoreSlim(1);
         _canReadHandle = new SemaphoreSlim(0);
      }

      #region Methods
      public void OnCompleted() => _canReadHandle.Release();
      public void OnError(Exception error)
      {
         _error = error;

         _canReadHandle.Release();
      }
      public void OnNext(T value)
      {
         _canWriteHandle.Wait();

         _latestValue = value;
         _yieldValue = true;

         _canReadHandle.Release();
      }

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
