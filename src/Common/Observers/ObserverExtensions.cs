using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TNO.Common.Observers;

/// <summary>
/// Provides extension classes that make use of the <see cref="AsyncObserver{T}"/>
/// and <see cref="AsyncEnumerableObserver{T}"/> classes, in order to turn the
/// <see href="https://learn.microsoft.com/en-us/dotnet/standard/events/observer-design-pattern">Observer Design Pattern</see>
/// into the <see href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/">Async/Await Pattern</see>.
/// </summary>
public static class ObserverExtensions
{
   #region Functions
   /// <summary>
   /// Converts the given <paramref name="subscribeDelegate"/> into a
   /// <see cref="Task{T}"/> that can be awaited in order to receive the last value.
   /// </summary>
   /// <typeparam name="T">The type that provides notification information.</typeparam>
   /// <param name="subscribeDelegate">The delegate used to subscribe to the <see cref="IObserver{T}"/>.</param>
   /// <returns>A task that can be used to asynchronously retrieve the last received value.</returns>
   public static Task<T> SingleAsync<T>(this Action<IObserver<T>> subscribeDelegate)
   {
      AsyncObserver<T> observer = new AsyncObserver<T>();
      subscribeDelegate.Invoke(observer);

      return observer.Task;
   }

   /// <summary>
   /// Converts the given <paramref name="subscribeDelegate"/> into a
   /// <see cref="IAsyncEnumerable{T}"/> that can be used to enumerate
   /// through all the received values.
   /// </summary>
   /// <typeparam name="T">The type that provides notification information.</typeparam>
   /// <param name="subscribeDelegate">The delegate used to subscribe to the <see cref="IObserver{T}"/>.</param>
   /// <param name="cancellationToken">A token that can be used to cancel this operation.</param>
   /// <returns>An asynchronous enumerable which can be used to iterate through all the received values.</returns>
   public static IAsyncEnumerable<T> EnumerateAsync<T>(this Action<IObserver<T>> subscribeDelegate, CancellationToken cancellationToken = default)
   {
      AsyncEnumerableObserver<T> observer = new AsyncEnumerableObserver<T>();
      subscribeDelegate.Invoke(observer);

      return observer.GetAsync(cancellationToken);
   }

   /// <summary>
   /// Converts the given <paramref name="subscriber"/> into a
   /// <see cref="Task{T}"/> that can be awaited in order to receive the last value.
   /// </summary>
   /// <inheritdoc cref="SingleAsync{T}(Action{IObserver{T}})"/>
   /// <param name="subscriber">The object to use to subscribe to the <see cref="IObserver{T}"/>.</param>
   public static Task<T> SingleAsync<T>(this IObservable<T> subscriber)
   {
      AsyncObserver<T> observer = new AsyncObserver<T>();
      subscriber.Subscribe(observer);

      return observer.Task;
   }

   /// <summary>
   /// Converts the given <paramref name="subscriber"/> into a
   /// <see cref="IAsyncEnumerable{T}"/> that can be used to
   /// enumerate through all the received values.
   /// </summary>
   /// <inheritdoc cref="EnumerateAsync{T}(Action{IObserver{T}}, CancellationToken)"/>
   /// <param name="subscriber">The object to use to subscribe to the <see cref="IObserver{T}"/>.</param>
   /// <param name="cancellationToken">A token that can be used to cancel this operation.</param>
   public static IAsyncEnumerable<T> EnumerateAsync<T>(this IObservable<T> subscriber, CancellationToken cancellationToken = default)
   {
      AsyncEnumerableObserver<T> observer = new AsyncEnumerableObserver<T>();
      subscriber.Subscribe(observer);

      return observer.GetAsync(cancellationToken);
   }
   #endregion
}
