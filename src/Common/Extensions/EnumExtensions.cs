using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TNO.Common.Extensions;

/// <summary>
/// Contains extension methods related to enums or the <see cref="Enum"/> class.
/// </summary>
public static class EnumExtensions
{
   #region Functions
   /// <summary>Gets all the values from the given <paramref name="enumType"/> in the ascending order.</summary>
   /// <param name="enumType">The type of the enum to get the values from.</param>
   /// <returns>The values defined in the given <paramref name="enumType"/>, in ascending order.</returns>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="enumType"/> is not an enum.</exception>
   public static object[] GetValuesAscending(Type enumType)
   {
      if (enumType.IsEnum == false)
         throw new ArgumentException($"The given type ({enumType}) is not an enum type.", nameof(enumType));

      Array values = Enum.GetValues(enumType);
      Array.Sort(values);

      return values
         .Cast<object>()
         .ToArray();
   }

   /// <summary>Gets all the values defined in the enum of the type <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The type of the enum to get the values from.</typeparam>
   /// <returns>The values defined in the enum of the type <typeparamref name="T"/>, in ascending order.</returns>
   public static T[] GetValuesAscending<T>() where T : struct, Enum
   {
#if NET5_0_OR_GREATER
      T[] arr = Enum.GetValues<T>();
      Array.Sort(arr);

      return arr;
#else
      Array values = Enum.GetValues(typeof(T));
      Array.Sort(values);

      return (T[])values;
#endif
   }

   /// <summary>
   /// Gets all the individual flags (in ascending order), from 
   /// the given combined <paramref name="flags"/> value.
   /// </summary>
   /// <typeparam name="T">The type of the enum.</typeparam>
   /// <param name="flags">The combined flags value.</param>
   /// <returns>An enumerable of the present flags, in ascending order.</returns>
   public static IEnumerable<T> SplitValuesAscending<T>(this T flags) where T : struct, Enum
   {
      // Todo(Nightowl): Should this only return single bit flag values?;

      T defaultValue = (T)Enum.ToObject(typeof(T), 0);
      T[] ascending = GetValuesAscending<T>();

      foreach (T possibleFlag in ascending)
      {
         if (possibleFlag.Equals(defaultValue))
            continue;

         if (flags.HasFlag(possibleFlag))
            yield return possibleFlag;
      }
   }

   /// <summary>Combines the given flag <paramref name="values"/> into a single enum value of the type <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The type of the enum flags.</typeparam>
   /// <param name="values">The enum flags to combine.</param>
   /// <returns>A single enum value that represents the combined flag <paramref name="values"/>.</returns>
   public static T CombineFlags<T>(this IEnumerable<T> values) where T : struct, Enum
   {
      Type enumType = typeof(T);
      Type underlyingType = enumType.GetEnumUnderlyingType();

      if (underlyingType == typeof(ulong))
         return CombineFlagsUInt64(values);

      long flags = 0;
      foreach (T flag in values)
         flags |= Convert.ToInt64(flag);

      return (T)Enum.ToObject(enumType, flags);
   }

   private static T CombineFlagsUInt64<T>(IEnumerable<T> values) where T : struct, Enum
   {
      Type enumType = typeof(T);
      Debug.Assert(enumType.GetEnumUnderlyingType() == typeof(ulong));

      ulong flags = 0;
      foreach (T flag in values)
         flags |= Convert.ToUInt64(flag);

      return (T)Enum.ToObject(enumType, flags);
   }
   #endregion
}
