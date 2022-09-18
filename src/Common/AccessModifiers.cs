using System;

namespace TNO.Common
{
   /// <summary>
   /// Contains the access modifier as defined on 
   /// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/access-modifiers">Access Modifiers - C# Reference</see>.
   /// </summary>
   [Flags]
   public enum AccessModifiers : byte
   {
      #region Basic
      /// <summary>Access is not restricted.</summary>
      Public = 1,

      /// <summary>Access is limited to the containing class or types derived from the containing class.</summary>
      Protected = 2,

      /// <summary>Access is limited to the current assembly.</summary>
      Internal = 4,

      /// <summary>Access is limited to the containing type.</summary>
      Private = 8,
      #endregion

      #region Valid Combinations
      /// <summary>Access is limited to the current assembly or types derived from the containing class.</summary>
      ProtectedInternal = Protected | Internal,

      /// <summary>Access is limited to the containing class or types derived from the containing class within the current assembly.</summary>
      PrivateProtected = Private | Protected,
      #endregion
   }
}
