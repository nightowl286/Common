using System;
using System.Diagnostics.CodeAnalysis;

namespace TNO.Common.Extensions
{
   public static class TypeExtensions
   {
      public static bool IsNullable(this Type type) => Nullable.GetUnderlyingType(type) is not null;
      public static bool ImplementsOpenInterface(this Type type, Type openInterface)
      {
         if (!openInterface.IsGenericTypeDefinition)
            return false;

         Type[] implementedInterfaces = type.GetInterfaces();

         foreach (Type implementedInterface in implementedInterfaces)
         {
            if (!implementedInterface.IsGenericType)
               continue;

            Type definition = implementedInterface.GetGenericTypeDefinition();

            if (definition == openInterface)
               return true;
         }

         return false;
      }
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
      public static bool IsSubclassOfDefinition(this Type type, Type genericDefnition, [NotNullWhen(true)] out Type? constructedGeneric)
      {
         if (!genericDefnition.IsGenericTypeDefinition)
            throw new ArgumentException($"The given definition ({genericDefnition}) was not a generic definition.", nameof(genericDefnition));

         do
         {
            if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == genericDefnition)
            {
               constructedGeneric = type;
               return true;
            }

            type = type.BaseType;
         }
         while (type != typeof(object));

         constructedGeneric = null;
         return false;
      }
   }
}
