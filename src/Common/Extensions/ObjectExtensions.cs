using System;

namespace TNO.Common.Extensions;

/// <summary>
/// Contains extension methods for the <see cref="object"/> class.
/// </summary>
public static class ObjectExtensions
{
   #region Functions
   /// <summary>
   /// Will dispose the given <paramref name="instance"/> if it
   /// implements the <see cref="IDisposable"/> interface.
   /// </summary>
   /// <param name="instance">The instance to try and dispose.</param>
   /// <remarks>
   /// This currently only checks for <see cref="IDisposable"/>,
   /// support for <see cref="IAsyncDisposable"/> might be added later.
   /// </remarks>
   public static void TryDispose(this object instance)
   {
      if (instance is IDisposable disposable)
         disposable.Dispose();
   }
   #endregion
}
