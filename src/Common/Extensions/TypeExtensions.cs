using System;

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
   }
}
