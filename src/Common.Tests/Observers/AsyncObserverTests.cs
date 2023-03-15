using TNO.Common.Observers;
using TNO.Tests.Common;

namespace TNO.Common.Tests.Observers;

[TestClass]
[TestCategory(Category.Observers)]
public class AsyncObserverTests
{
   #region Tests
   [TestMethod]
   public async Task Task_WithSingleValue_ReturnsValue()
   {
      // Arrange
      int expected = 1;
      AsyncObserver<int> sut = new AsyncObserver<int>();

      // Act
      sut.OnNext(expected);
      sut.OnCompleted();
      int result = await sut.Task;

      // Assert
      Assert.AreEqual(expected, result);
   }

   [TestMethod]
   public async Task Task_WithMultipleValues_ReturnsLatestValue()
   {
      // Arrange
      int first = 1;
      int expected = 2;
      AsyncObserver<int> sut = new AsyncObserver<int>();

      // Act
      sut.OnNext(first);
      sut.OnNext(expected);
      sut.OnCompleted();
      int result = await sut.Task;

      // Assert
      Assert.AreEqual(expected, result);
   }

   [TestMethod]
   public async Task Task_WithNullableTypeAndNoValue_ReturnsNull()
   {
      // Arrange
      AsyncObserver<int?> sut = new AsyncObserver<int?>();

      // Act
      sut.OnCompleted();
      int? result = await sut.Task;

      // Assert
      Assert.IsNull(result);
   }

   [TestMethod]
   public async Task Task_WithNonNullableTypeAndNoValue_ReturnsDefault()
   {
      // Arrange
      int expected = default;
      AsyncObserver<int> sut = new AsyncObserver<int>();

      // Act
      sut.OnCompleted();
      int result = await sut.Task;

      // Assert
      Assert.AreEqual(expected, result);
   }

   [TestMethod]
   public async Task Task_WithError_ThrowsCorrectException()
   {
      // Arrange
      AsyncObserver<int> sut = new AsyncObserver<int>();

      // Act
      sut.OnError(new TestException());
      async Task Act() => await sut.Task;

      // Assert
      await Assert.ThrowsExceptionAsync<TestException>(Act);
   }

   [TestMethod]
   public async Task Task_WithErrorAndValue_ThrowsCorrectException()
   {
      // Arrange
      AsyncObserver<int> sut = new AsyncObserver<int>();

      // Act
      sut.OnError(new TestException());
      sut.OnNext(default);
      async Task Act() => await sut.Task;

      // Assert
      await Assert.ThrowsExceptionAsync<TestException>(Act);
   }

   [TestMethod]
   public void Task_CompletedAndThenErrored_ThrowsCorrectExceptionOnError()
   {
      // Arrange
      AsyncObserver<int> sut = new AsyncObserver<int>();

      // Act
      sut.OnCompleted();
      void Act() => sut.OnError(new TestException());

      // Assert
      Assert.ThrowsException<TestException>(Act);
   }
   #endregion
}
