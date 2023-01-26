using System.Reflection;
using TNO.Common.Extensions;

namespace TNO.Common.Tests.Extensions;

[TestClass]
[TestCategory(Category.Extensions)]
public class EnumExtensionsTests
{
   #region Test Methods

   #region Get Values Ascending
   [DynamicData(
      nameof(GetAllOrderingEnums),
      DynamicDataSourceType.Method,
      DynamicDataDisplayName = nameof(GetAllOrderingEnumsTestDisplayNames))]
   [DataTestMethod]
   public void GetValuesAscending_WithEnumType_IsInAscendingOrder(Type enumType)
   {
      // Act
      object[] result = EnumExtensions.GetValuesAscending(enumType);

      // Assert
      AssertArrayIsAscending(result);
   }

   [TestMethod]
   public void GetValuesAscendingGeneric_EnumAllPositiveAscending_IsInAscendingOrder() => GetValuesAscending_GenericTestBase<EnumAllPositiveAscending>();
   [TestMethod]
   public void GetValuesAscendingGeneric_EnumAllPositiveDescending_IsInAscendingOrder() => GetValuesAscending_GenericTestBase<EnumAllPositiveDescending>();

   [TestMethod]
   public void GetValuesAscendingGeneric_EnumAllNegativeAscending_IsInAscendingOrder() => GetValuesAscending_GenericTestBase<EnumAllNegativeAscending>();
   [TestMethod]
   public void GetValuesAscendingGeneric_EnumAllNegativeDescending_IsInAscendingOrder() => GetValuesAscending_GenericTestBase<EnumAllNegativeDescending>();

   [TestMethod]
   public void GetValuesAscendingGeneric_EnumMixedAscending_IsInAscendingOrder() => GetValuesAscending_GenericTestBase<EnumMixedAscending>();
   [TestMethod]
   public void GetValuesAscendingGeneric_EnumMixedDescending_IsInAscendingOrder() => GetValuesAscending_GenericTestBase<EnumMixedDescending>();

   private void GetValuesAscending_GenericTestBase<T>() where T : struct, Enum
   {
      // Act
      T[] result = EnumExtensions.GetValuesAscending<T>();

      // Assert
      AssertArrayIsAscending(result);
   }
   private void AssertArrayIsAscending(Array array)
   {
      if (array.Length < 2) return;
      if (array.GetValue(0) is not IComparable last)
      {
         Assert.Fail($"The first element was either null or does not implement {typeof(IComparable)}.");
         return; // required because of analysis failure.
      }

      for (int i = 1; i < array.Length; i++)
      {
         if (array.GetValue(1) is not IComparable current)
         {
            Assert.Fail($"The element at the index {i:n0} was either null or does not implement {typeof(IComparable)}.");
            return; // required because of analysis failure.
         }

         int comp = last.CompareTo(current);
         if (comp > 0)
         {
            Assert.Fail($"The objects at the indecies ({i - 1:n0}), ({i:n0}) with the values ({last}), ({current}) were not in ascending order.");
            return;
         }
         last = current;
      }
   }
   #endregion

   #region Combine Flags
   [TestMethod]
   public void CombineFlags_WithInt_Successful()
   {
      // Arrange
      EnumFlagsInt[] flags = new[]
      {
         EnumFlagsInt.One,
         EnumFlagsInt.Two,
         EnumFlagsInt.Four,
         EnumFlagsInt.Eight,
      };

      // Act
      EnumFlagsInt result = EnumExtensions.CombineFlags(flags);

      // Assert
      Assert.AreEqual(EnumFlagsInt.All, result);
   }

   [TestMethod]
   public void CombineFlags_WithULong_Successful()
   {
      // Arrange
      EnumFlagsUInt64[] flags = new[]
      {
         EnumFlagsUInt64.One,
         EnumFlagsUInt64.Two,
         EnumFlagsUInt64.Four,
         EnumFlagsUInt64.Eight,
         EnumFlagsUInt64.Large,
      };

      // Act
      EnumFlagsUInt64 result = EnumExtensions.CombineFlags(flags);

      // Assert
      Assert.AreEqual(EnumFlagsUInt64.All, result);
   }
   #endregion

   #region Split Values
   [TestMethod]
   public void SplitValuesAscending_WithCombinedFlags_GetsSetFlags()
   {
      // Arrange
      EnumFlagsInt combinedFlags = EnumFlagsInt.Four | EnumFlagsInt.One | EnumFlagsInt.Two;
      EnumFlagsInt[] expected = new[] { EnumFlagsInt.One, EnumFlagsInt.Two, EnumFlagsInt.Four };

      // Act
      EnumFlagsInt[] result = EnumExtensions
         .SplitValuesAscending(combinedFlags)
         .ToArray();

      // Assert
      CollectionAssert.AreEqual(expected, result);
   }
   #endregion
   #endregion

   #region Helpers
   private static IEnumerable<object[]> GetAllOrderingEnums()
   {
      yield return new[] { typeof(EnumAllPositiveAscending) };
      yield return new[] { typeof(EnumAllPositiveDescending) };

      yield return new[] { typeof(EnumAllNegativeAscending) };
      yield return new[] { typeof(EnumAllNegativeDescending) };

      yield return new[] { typeof(EnumMixedAscending) };
      yield return new[] { typeof(EnumMixedDescending) };
   }
   public static string GetAllOrderingEnumsTestDisplayNames(MethodInfo _, object[] values)
   {
      Type enumType = (Type)values[0];
      return enumType.Name;
   }
   #endregion

   #region Test Enums
   #region Ordering Enums
   private enum EnumAllPositiveAscending
   {
      A = 1,
      B = 2,
      C = 3,
   }
   private enum EnumAllPositiveDescending
   {
      C = 3,
      B = 2,
      A = 1,
   }
   private enum EnumAllNegativeAscending
   {
      A = -3,
      B = -2,
      C = -1,
   }
   private enum EnumAllNegativeDescending
   {
      C = -1,
      B = -2,
      A = -3,
   }
   private enum EnumMixedAscending
   {
      A = -1,
      B = 0,
      C = 1,
   }
   private enum EnumMixedDescending
   {
      C = 1,
      B = 0,
      A = -1
   }
   #endregion

   #region Flag Enums
   private enum EnumFlagsInt : int
   {
      None = 0,
      One = 1,
      Two = 2,
      Four = 4,
      Eight = 8,
      All = One | Two | Four | Eight
   }
   private enum EnumFlagsUInt64 : ulong
   {
      None = 0,
      One = 1,
      Two = 2,
      Four = 4,
      Eight = 8,
      Large = 9_223_372_036_854_775_808,
      All = One | Two | Four | Eight | Large
   }
   #endregion
   #endregion
}
