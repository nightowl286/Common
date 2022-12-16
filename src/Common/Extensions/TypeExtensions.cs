using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TNO.Common.Extensions
{
   /// <summary>
   /// Contains extension methods for the <see cref="Type"/> class.
   /// </summary>
   public static class TypeExtensions
   {
      /// <summary>Checks whether the given <paramref name="type"/> is a nullable type.</summary>
      /// <param name="type">The type to check.</param>
      /// <returns>
      /// <see langword="true"/> if the given <paramref name="type"/> 
      /// is a nullable type, otherwise <see langword="false"/>.
      /// </returns>
      /// <remarks>
      /// This method calls <see cref="Nullable.GetUnderlyingType(Type)"/>,
      /// use that instead if you also require the non-nullable type.
      /// </remarks>
      public static bool IsNullable(this Type type) => Nullable.GetUnderlyingType(type) is not null;

      /// <summary>
      /// Checks whether the given <paramref name="type"/> implements 
      /// the given <paramref name="openInterface"/>.
      /// </summary>
      /// <param name="type">The type to check.</param>
      /// <param name="openInterface">
      /// The type of the open interface, this means a non-constructed generic interface.
      /// </param>
      /// <returns>
      /// <see langword="true"/> if the given <paramref name="type"/> implements
      /// the given <paramref name="openInterface"/>, <see langword="false"/> otherwise.<br/><br/>
      /// Also <see langword="false"/> if the given <paramref name="openInterface"/> 
      /// is not a generic type definition.
      /// </returns>
      public static bool ImplementsOpenInterface(this Type type, Type openInterface)
      {
         IEnumerable<Type> implemented = GetOpenInterfaceImplementations(type, openInterface);

         return implemented.Any();
      }

      /// <summary>Whether or an instance of the given <paramref name="type"/> can be created.</summary>
      /// <param name="type">The type to check.</param>
      /// <param name="ignoreTypeDefinition">
      /// Whether to ignore if the given <paramref name="type"/> is a generic 
      /// type definition, but to still continue the further checks.
      /// </param>
      /// <returns><see langword="true"/> if the given <paramref name="type"/> can be constructed, <see langword="false"/> otherwise.</returns>
      /// <remarks>
      /// This method performs the following checks (in the specified order).
      /// <list type="number">
      ///   <item>
      ///      If the given <paramref name="type"/> is a generic type definition return <see langword="false"/>, 
      ///      unless <see langword="true"/> is given as <paramref name="ignoreTypeDefinition"/>.
      ///      </item>
      ///   <item>
      ///      Return <see langword="false"/> if the given <paramref name="type"/> is 
      ///      <see langword="abstract"/>, an <see langword="interface"/> or an <see langword="enum"/>.
      ///   </item>
      ///   <item>Returns <see langword="true"/> if none of the above conditions apply.</item>
      /// </list>
      /// </remarks>
      public static bool CanCreateInstance(this Type type, bool ignoreTypeDefinition = false)
      {
         bool isTypeDefinition = ignoreTypeDefinition ? false : type.IsGenericTypeDefinition;

         if (isTypeDefinition
            || type.IsInterface
            || type.IsAbstract
            || type.IsEnum)
            return false;

         return true;
      }

      /// <summary>Whether the given <paramref name="type"/> inherits the given <paramref name="genericDefinition"/>.</summary>
      /// <param name="type">The type to check.</param>
      /// <param name="genericDefinition">The generic definition to check for.</param>
      /// <param name="constructedGeneric">
      /// The constructed generic type that the given <paramref name="type"/> inherits.<br/><br/>
      /// Will only be <see langword="null"/> if this method returns <see langword="false"/>.
      /// </param>
      /// <returns>
      /// <see langword="true"/> if the given <paramref name="type"/> inherits the 
      /// given <paramref name="genericDefinition"/>, <see langword="false"/> otherwise.
      /// </returns>
      /// <exception cref="ArgumentException">
      /// Thrown if the given <paramref name="genericDefinition"/> is not actually a generic type definition.
      /// </exception>
      public static bool IsSubclassOfDefinition(this Type type, Type genericDefinition, [NotNullWhen(true)] out Type? constructedGeneric)
      {
         if (!genericDefinition.IsGenericTypeDefinition)
            throw new ArgumentException($"The given definition ({genericDefinition}) was not a generic definition.", nameof(genericDefinition));

         while (type != typeof(object))
         {
            if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == genericDefinition)
            {
               constructedGeneric = type;
               return true;
            }

            Debug.Assert(type.BaseType != null);
            type = type.BaseType;
         }

         constructedGeneric = null;
         return false;
      }

      /// <summary>
      /// Returns all the interfaces implemented by the given <paramref name="type"/>
      /// that match the given <paramref name="openInterface"/>.
      /// </summary>
      /// <param name="type">The type to check.</param>
      /// <param name="openInterface">The open interface to check for implementations of.</param>
      /// <returns>An enumerable of all the open interface implementations.</returns>
      public static IEnumerable<Type> GetOpenInterfaceImplementations(this Type type, Type openInterface)
      {
         if (!openInterface.IsGenericTypeDefinition)
            yield break;

         Type[] implementedInterfaces = type.GetInterfaces();

         foreach (Type implementedInterface in implementedInterfaces)
         {
            if (!implementedInterface.IsGenericType)
               continue;

            Type definition = implementedInterface.GetGenericTypeDefinition();

            if (definition == openInterface)
               yield return implementedInterface;
         }
      }
   }
}
