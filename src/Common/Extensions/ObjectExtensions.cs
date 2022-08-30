using System;

namespace TNO.Common.Extensions
{
   public static class ObjectExtensions
   {
      #region Functions
      public static void TryDispose(this object instance)
      {
         if (instance is IDisposable disposable)
            disposable.Dispose();
      }
      #endregion
   }
}
