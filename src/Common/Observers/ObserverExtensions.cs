using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TNO.Common.Observers
{
   public static class ObserverExtensions
   {
      #region Functions
      public static Task<T> SingleAsync<T>(this Action<IObserver<T>> subscribeDelegate)
      {
         AsyncObserver<T> observer = new AsyncObserver<T>();
         subscribeDelegate.Invoke(observer);

         return observer.Task;
      }

      public static IAsyncEnumerable<T> EnumerateAsync<T>(this Action<IObserver<T>> subscribeDelegate, CancellationToken cancellationToken = default)
      {
         AsyncEnumerableObserver<T> observer = new AsyncEnumerableObserver<T>();
         subscribeDelegate.Invoke(observer);

         return observer.GetAsync(cancellationToken);
      }

      public static Task<T> SingleAsync<T>(this IObservable<T> subscriber)
      {
         AsyncObserver<T> observer = new AsyncObserver<T>();
         subscriber.Subscribe(observer);

         return observer.Task;
      }

      public static IAsyncEnumerable<T> EnumerateAsync<T>(this IObservable<T> subscriber, CancellationToken cancellationToken = default)
      {
         AsyncEnumerableObserver<T> observer = new AsyncEnumerableObserver<T>();
         subscriber.Subscribe(observer);

         return observer.GetAsync(cancellationToken);
      }
      #endregion
   }
}
