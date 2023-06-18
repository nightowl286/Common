using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace TNO.Common.Reflection;

/// <summary>
/// Contains extension methods for the <see cref="ParameterInfo"/> class.
/// </summary>
public static class ParameterInfoExtensions
{
   #region Functions
#if NET5_0_OR_GREATER
#pragma warning disable CS0419 // Weird bug? There is no ambiguity. https://github.com/dotnet/roslyn/issues/51013
#endif
   /// <summary>Checks whether the given <paramref name="parameter"/> is nullable.</summary>
   /// <param name="parameter">The parameter info to check.</param>
   /// <returns>
   /// <see langword="true"/> if the given <paramref name="parameter"/> 
   /// is nullable, otherwise <see langword="false"/>.
   /// </returns>
   /// <remarks>
   /// This method performs the following checks (in the specified order).
   /// <list type="number">
   ///   <item>Returns <see langword="false"/> if the <see cref="DisallowNullAttribute"/> attribute is present.</item>
   ///   <item>Whether the <see cref="TypeExtensions.IsNullable(Type)"/> method call returns <see langword="true"/>.</item>
   ///   <item>Whether the <see cref="AllowNullAttribute"/> is present.</item>
   /// </list>
   /// </remarks>
#if NET5_0_OR_GREATER
#pragma warning restore CS0419
#endif
   public static bool IsNullable(this ParameterInfo parameter)
   {
      if (parameter.GetCustomAttribute<DisallowNullAttribute>() is not null)
         return false;

      if (parameter.ParameterType.IsNullable())
         return true;

      return parameter.GetCustomAttribute<AllowNullAttribute>() is not null;
   }
   #endregion
}
