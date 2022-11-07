using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace TNO.Common.Extensions
{
   /// <summary>
   /// Contains extension methods for the <see cref="ParameterInfo"/> class.
   /// </summary>
   public static class ParameterInfoExtensions
   {
      #region Functions
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
      ///   <item>Whether the <see cref="TypeExtensions.IsNullable(System.Type)"/> method call returns <see langword="true"/>.</item>
      ///   <item>Whether the <see cref="AllowNullAttribute"/> is present.</item>
      /// </list>
      /// </remarks>
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
}
