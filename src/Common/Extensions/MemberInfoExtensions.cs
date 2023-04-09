using System;
using System.Reflection;

namespace TNO.Common.Extensions;

/// <summary>
/// Contains useful extension methods related to the <see cref="MemberInfo"/>.
/// </summary>
public static class MemberInfoExtensions
{
   #region Methods
   /// <summary>
   /// Checks whether the given <paramref name="memberInfo"/> has defined 
   /// an <see cref="Attribute"/> of the type <typeparamref name="T"/>.
   /// </summary>
   /// <typeparam name="T">The type of the <see cref="Attribute"/>.</typeparam>
   /// <param name="memberInfo">The member to check.</param>
   /// <param name="inherit">
   /// <see langword="true"/> to search the inheritance chain of the <paramref name="memberInfo"/> to find the attributes,
   /// <see langword="false"/> otherwise. This parameter is ignored for properties and events.
   /// </param>
   /// <returns>
   /// <see langword="true"/> if one or more instances of the <see cref="Attribute"/> of type <typeparamref name="T"/>
   /// or any of its derived types is applied to this member, <see langword="false"/> otherwise.
   /// </returns>
   public static bool IsDefined<T>(this MemberInfo memberInfo, bool inherit) where T : Attribute
      => memberInfo.IsDefined(typeof(T), inherit);

   /// <summary>
   /// Checks whether the given <paramref name="memberInfo"/> has defined
   /// an <see cref="Attribute"/> of the type <typeparamref name="T"/>.
   /// </summary>
   /// <typeparam name="T">The type of the <see cref="Attribute"/>.</typeparam>
   /// <param name="memberInfo">The member to check.</param>
   /// <returns>
   ///  <see langword="true"/> if an <see cref="Attribute"/> of the type <typeparamref name="T"/> 
   ///  is applied to the given <paramref name="memberInfo"/>, <see langword="false"/> otherwise.
   /// </returns>
   public static bool IsDefined<T>(this MemberInfo memberInfo) where T : Attribute
      => memberInfo.IsDefined(typeof(T));
   #endregion
}
