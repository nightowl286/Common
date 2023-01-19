using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using TNO.Common.Extensions;

namespace TNO.Common.Tests.Extensions;

[TestClass]
[TestCategory(Category.Extensions)]
public class ParameterInfoExtensionsTests
{
   #region Test Methods
   [DataRow(nameof(TestMethod_Nullable_Parameter1))]
   [DataRow(nameof(TestMethod_Nullable_Parameter2))]
   [DataRow(nameof(TestMethod_Nullable_Parameter3))]
   [DataRow(nameof(TestMethod_Nullable_Parameter4))]
   [DataRow(nameof(TestMethod_Nullable_Parameter5))]
   [TestMethod()]
   public void IsNullable_NullableType_ReturnsTrue(string methodName)
   {
      // Arrange
      ParameterInfo parameter = GetParamterInfo(methodName);

      // Act
      bool isNullable = ParameterInfoExtensions.IsNullable(parameter);

      // Assert
      Assert.IsTrue(isNullable);
   }

   [DataRow(nameof(TestMethod_NonNullable_Parameter1))]
   [DataRow(nameof(TestMethod_NonNullable_Parameter2))]
   [DataRow(nameof(TestMethod_NonNullable_Parameter3))]
   [DataRow(nameof(TestMethod_NonNullable_Parameter4))]
   [DataRow(nameof(TestMethod_NonNullable_Parameter5))]
   [DataRow(nameof(TestMethod_NonNullable_Parameter6))]
   [TestMethod]
   public void IsNullable_NonNullableType_ReturnsFalse(string methodName)
   {
      // Arrange
      ParameterInfo parameter = GetParamterInfo(methodName);

      // Act
      bool isNullable = ParameterInfoExtensions.IsNullable(parameter);

      // Assert
      Assert.IsFalse(isNullable);
   }

   public static void TestMethod_Nullable_Parameter1(int? _) { }
   public static void TestMethod_Nullable_Parameter2([AllowNull] string? _) { }
   public static void TestMethod_Nullable_Parameter3(long? _) { }
   public static void TestMethod_Nullable_Parameter4([AllowNull] int? _) { }
   public static void TestMethod_Nullable_Parameter5([AllowNull] long? _) { }

   public static void TestMethod_NonNullable_Parameter1(int _) { }
   public static void TestMethod_NonNullable_Parameter2(string _) { }
   public static void TestMethod_NonNullable_Parameter3(long _) { }
   public static void TestMethod_NonNullable_Parameter4([DisallowNull] string? _) { }
   public static void TestMethod_NonNullable_Parameter5([DisallowNull, AllowNull] string? _) { }
   public static void TestMethod_NonNullable_Parameter6([DisallowNull] int? _) { }
   #endregion

   #region Helpers
   private static ParameterInfo GetParamterInfo(string methodName)
   {
      MethodInfo method = GetMethodInfo(methodName);

      return method.GetParameters().Single();
   }
   private static MethodInfo GetMethodInfo(string name)
   {
      Type classType = typeof(ParameterInfoExtensionsTests);
      MethodInfo? method = classType.GetMethod(name, BindingFlags.Public | BindingFlags.Static);
      Assert.IsNotNull(method);

      return method;
   }
   #endregion
}
