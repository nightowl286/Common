using TNO.Common.Observers;
using TNO.Common.Tasks;
using TNO.Tests.Common;
using TNO.Tests.Common.Extensions;

namespace TNO.Common.Tests.Observers
{
   [TestClass]
   [TestCategory(Category.Observers)]
   public class AsyncEnumerableObserverTests
   {
      #region Tests
      [TestMethod]
      public async Task GetAsync_NoValues_ReturnsEmptyEnumerable()
      {
         // Arrange
         AsyncEnumerableObserver<int> sut = new AsyncEnumerableObserver<int>();

         // Act
         sut.OnCompleted();
         IAsyncEnumerable<int> enumerable = sut.GetAsync();

         // Pre Assert
         int[] results = await enumerable.ToArrayAsync();

         // Assert
         CollectionAssert.That.IsEmpty(results);
      }

      [TestMethod]
      public async Task GetAsync_OneValue_ReturnsCorrectValue()
      {
         // Arrange
         int expectedResult = 1;
         int expectedSize = 1;
         AsyncEnumerableObserver<int> sut = new AsyncEnumerableObserver<int>();

         // Act
         sut.OnNext(expectedResult);
         sut.OnCompleted();
         IAsyncEnumerable<int> enumerable = sut.GetAsync();

         // Pre Assert
         int[] results = await enumerable.ToArrayAsync();

         // Assert
         CollectionAssert.That.IsOfSize(results, expectedSize);
         int result = results.Single();
         Assert.AreEqual(expectedResult, result);
      }

      [TestMethod]
      public async Task GetAsync_MultipleValues_ReturnsInCorrectOrder()
      {
         // Arrange
         int[] expected = new int[] { 1, 2, 3, 4, 5 };
         AsyncEnumerableObserver<int> sut = new AsyncEnumerableObserver<int>();

         // Act
         foreach (int value in expected)
            sut.OnNext(value);
         sut.OnCompleted();

         IAsyncEnumerable<int> enumerable = sut.GetAsync();

         // Pre Assert
         int[] results = await enumerable.ToArrayAsync();

         // Assert
         CollectionAssert.AreEqual(expected, results);
      }

      [TestMethod]
      public async Task GetAsync_Cancellation_StopsEnumerationAtCorrectPoint()
      {
         // Arrange
         int[] values = new int[] { 1, 2, 3, 4, 5 };
         int[] expected = new int[] { 1, 2 };
         AsyncEnumerableObserver<int> sut = new AsyncEnumerableObserver<int>();
         CancellationTokenSource tokenSource = new CancellationTokenSource();

         // Act
         foreach (int value in values)
            sut.OnNext(value);
         sut.OnCompleted();

         IAsyncEnumerable<int> enumerable = sut.GetAsync(tokenSource.Token);
         List<int> actual = new List<int>();
         await foreach (int value in enumerable)
         {
            actual.Add(value);
            if (actual.Count == 2)
               tokenSource.Cancel();
         }

         // Assert
         CollectionAssert.AreEqual(expected, actual, $"Actual collection ({actual.Count}) has a different enumeration count than expected ({expected.Length}).");
      }

      [TestMethod]
      public async Task GetAsync_Error_ThrowsCorrectException()
      {
         // Arrange
         AsyncEnumerableObserver<int> sut = new AsyncEnumerableObserver<int>();

         // Act
         sut.OnError(new TestException());

         async Task Act()
         {
            await foreach (int value in sut.GetAsync()) { }
         }

         // Assert
         await Assert.ThrowsExceptionAsync<TestException>(Act);
      }

      [TestMethod]
      public async Task GetAsync_ObjectDisposed_ThrowObjectDisposedException()
      {
         // Arrange
         AsyncEnumerableObserver<int> sut = new AsyncEnumerableObserver<int>();
         sut.OnCompleted();
         _ = await sut.GetAsync().ToArrayAsync();

         // Act
         async Task Act() => await sut.GetAsync().ToArrayAsync();

         // Assert
         await Assert.ThrowsExceptionAsync<ObjectDisposedException>(Act);
      }
      #endregion
   }
}