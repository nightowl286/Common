using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace TNO.Common.Extensions
{
   public static class ParameterInfoExtensions
   {
      #region Functions
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
