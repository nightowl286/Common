using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace TNO.Common.Extensions;

/// <summary>
/// Contains helpful extension methods that can check whether an <see cref="Assembly"/> is optimised or not.
/// </summary>
public static class OptimisedAssemblyExtensions
{
   #region Is Optimised
   /// <summary>Checks whether the given <paramref name="assembly"/> is optimised.</summary>
   /// <param name="assembly">The assembly to check.</param>
   /// <returns>
   /// <see langword="true"/> if code in the given <paramref name="assembly"/>
   /// can get optimised, <see langword="false"/> otherwise.
   /// </returns>
   public static bool IsOptimised(this Assembly assembly)
   {
      IEnumerable<DebuggableAttribute> attributes = assembly.GetCustomAttributes<DebuggableAttribute>();
      foreach (DebuggableAttribute attr in attributes)
      {
         if (attr.IsJITOptimizerDisabled == false)
            return true;
      }

      return false;
   }

   /// <summary>Checks whether the given <paramref name="type"/> is in an optimised <see cref="Assembly"/>.</summary>
   /// <param name="type">The type to check.</param>
   /// <returns>
   /// <see langword="true"/> if the given <paramref name="type"/> is in an
   /// optimised <see cref="Assembly"/>, <see langword="false"/> otherwise.
   /// </returns>
   public static bool IsInOptimisedAssembly(this Type type)
      => IsOptimised(type.Assembly);

   /// <summary>Checks whether the given <paramref name="member"/> is in an optimised <see cref="Assembly"/>.</summary>
   /// <param name="member">The method base to check.</param>
   /// <returns>
   /// <see langword="true"/> if the given <paramref name="member"/> is in an
   /// optimised <see cref="Assembly"/>, <see langword="false"/> otherwise.
   /// </returns>
   public static bool IsInOptimisedAssembly(this MemberInfo member)
   {
      if (member.DeclaringType is null)
      {
         return IsOptimised(member.Module.Assembly);
      }

      return IsOptimised(member.DeclaringType.Assembly);
   }
   #endregion
}
