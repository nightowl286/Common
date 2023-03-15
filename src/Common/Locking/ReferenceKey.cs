#if NET5_0_OR_GREATER
using System.Threading;
#endif

namespace TNO.Common.Locking;

/// <summary>
/// Denotes a reference-only key that can be used in checks.
/// </summary>
public class ReferenceKey
{
   #region Fields
#if !NET5_0_OR_GREATER
   private static readonly object Lock = new object();
#endif
   private static ulong NextID;
   #endregion

   #region Properties
   /// <summary>The ID of this key.</summary>
   /// <remarks>
   /// This is meant only for debugging purposes and
   /// does not factor into the locking mechanism.
   /// </remarks>
   public ulong ID { get; }
   #endregion

   /// <summary>Creates a new reference key.</summary>
   public ReferenceKey()
   {
#if NET5_0_OR_GREATER
      ID = Interlocked.Increment(ref NextID);
#else
      lock (Lock)
      {
         ID = NextID++;
      }
#endif
   }

   #region Methods
   /// <summary>Gets this key's ID formatted as a hex <see cref="string"/>.</summary>
   /// <returns>The ID of this key.</returns>
   public string GetID() => $"{ID:X2}";

   /// <inheritdoc/>
   public override string ToString() => $"Key{{{GetID()}}}";
   #endregion
}
