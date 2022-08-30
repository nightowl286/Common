using System;

namespace TNO.Common
{
   /// <summary>
   /// An attribute used to denote that the marked member/class 
   /// is only meant to be accessible for their unit tests.
   /// </summary>
   [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
   public class TestOnlyAttribute : Attribute { }
}
