using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace TNO.Common.Disposable
{
   /// <summary>
   /// A base implementation that uses the correct <see href="https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose">dispose pattern</see>.
   /// </summary>
   /// <remarks>
   /// Override any of these 3 methods to provide functionality.
   /// <list type="bullet">
   ///   <item><see cref="DisposeManaged"/> to dispose managed resources.</item>
   ///   <item><see cref="DisposeUnmanaged"/> to dispose unmanaged resources.</item>
   ///   <item>
   ///      Or implement a finalizer and call <see cref="Dispose(bool)"/> with <see langword="false"/>
   ///      if you have also overridden <see cref="DisposeUnmanaged"/>.
   ///   </item>
   /// </list>
   /// </remarks>
   public abstract class DisposableBase : IDisposable
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

      /// <summary>Throws the <see cref="ObjectDisposedException"/> if this object has been disposed.</summary>
      /// <param name="objectName">The name of this object.</param>
      [MethodImpl(MethodImplOptions.NoInlining)]
      protected void ThrowIfDisposed(string objectName)
      {
         if (_disposed)
            ThrowObjectDisposed(objectName);
      }
      #endregion

      #region Functions
      /// <summary>Throws the <see cref="ObjectDisposedException"/> with the given <paramref name="objectName"/>.</summary>
      /// <param name="objectName">The name of the object that was disposed.</param>
      [DoesNotReturn]
      [MethodImpl(MethodImplOptions.NoInlining)]
      protected static void ThrowObjectDisposed(string objectName)
      {
         throw new ObjectDisposedException(objectName, $"This object ({objectName}) has been disposed and should no longer be accessed.");
      }
      #endregion
   }
}
