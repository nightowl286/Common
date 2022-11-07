using System;
using System.Diagnostics;

namespace TNO.Common
{
   /// <summary>
   /// An attribute used to denote that the marked target
   /// is only meant to be accessible for their unit tests.
   /// </summary>
   /// <remarks>
   /// In the future, if the actual usage of the marked target doesn't follow the <see cref="TreatAs"/> 
   /// <see cref="AccessModifiers"/> a custom analyser will report this as errors.
   /// </remarks>
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

      /// <summary>Creates a new instance of the <see cref="TestOnlyAttribute"/>.</summary>
      /// <param name="treatAs">
      /// The <see cref="AccessModifiers"/> that the target of this attribute should actually follow.
      /// </param>
      public TestOnlyAttribute(AccessModifiers treatAs) => TreatAs = treatAs;
   }
}
