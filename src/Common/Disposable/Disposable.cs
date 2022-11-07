using System;

namespace TNO.Common.Disposable
{
   /// <summary>
   /// A base implementation that uses the correct <see href="https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose">dispose pattern</see>.
   /// </summary>
   /// <remarks>
   /// Override any of these 3 methods to provide functionality.
   /// <list type="bullet">
   ///   <item><see cref="DisposeManaged"/> to dispose managed resources.</item>
   ///   <item><see cref="DisposeUnmanaged"/> to dispose unmanages resources.</item>
   ///   <item>
   ///      Or implement a finalizer and call <see cref="Dispose(bool)"/> with <see langword="false"/>
   ///      if you have also overriden <see cref="DisposeUnmanaged"/>.
   ///   </item>
   /// </list>
   /// </remarks>
   public abstract class Disposable : IDisposable
   {
      #region Fields
      private bool _disposed = false;
      #endregion

      #region Methods
      /// <inheritdoc/>
      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      /// <summary>Handles disposing managed and unmanaged resources.</summary>
      /// <param name="disposeManaged">Whether to dispose managed resources, unmanaged resources will always be disposed.</param>
      protected void Dispose(bool disposeManaged)
      {
         if (_disposed)
            return;

         _disposed = true;

         if (disposeManaged)
            DisposeManaged();

         DisposeUnmanaged();
      }

      /// <summary>Override to dispose managed resources.</summary>
      protected virtual void DisposeManaged() { }

      /// <summary>Override to dispose unmanaged resources.</summary>
      protected virtual void DisposeUnmanaged() { }
      #endregion
   }
}
