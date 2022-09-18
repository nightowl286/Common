using System;
using System.Diagnostics;

namespace TNO.Common
{
   /// <summary>
   /// An attribute used to denote that the marked member/class 
   /// is only meant to be accessible for their unit tests.
   /// </summary>
   [AttributeUsage(
      AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Field | 
      AttributeTargets.Interface | AttributeTargets.Constructor | AttributeTargets.Delegate |
      AttributeTargets.Property | AttributeTargets.Struct | AttributeTargets.Enum,
      AllowMultiple = false, Inherited = false)]
   [Conditional("DEBUG")]
   public class TestOnlyAttribute : Attribute 
   {
      #region Properties
      /// <summary>Gets the <see cref="AccessModifiers"/> that the marked element should be treated as instead.</summary>
      public AccessModifiers TreatAs { get; }
      #endregion
      public TestOnlyAttribute(AccessModifiers treatAs) => TreatAs = treatAs;
   }
}
