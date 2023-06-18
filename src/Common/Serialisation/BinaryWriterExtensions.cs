using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;

namespace TNO.Common.Serialisation;

/// <summary>
/// Contains useful extension methods related to the <see cref="BinaryWriter"/>.
/// </summary>
public static class BinaryWriterExtensions
{
   #region 7 Bit Encoded Integers
   /// <summary>Writes a 32-bit integer in a compressed format.</summary>
   /// <param name="writer">The writer to use.</param>
   /// <param name="value">The 32-bit integer to write.</param>
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public static void Write7BitEncodedInt(this BinaryWriter writer, int value)
   {
#if NET6_0_OR_GREATER
      writer.Write7BitEncodedInt(value);
#else
      Write7BitEncodedUInt(writer, (uint)value);
#endif
   }

   /// <summary>Writes a 32-bit unsigned integer in a compressed format.</summary>
   /// <param name="writer">The writer to use.</param>
   /// <param name="value">The 32-bit unsigned integer to write.</param>
   public static void Write7BitEncodedUInt(this BinaryWriter writer, uint value)
   {
      // Note(Nightowl): This implementation was copied from the original as it was only made public in .NET 6.0;

      while (value > 0x7Fu)
      {
         writer.Write((byte)(value | ~0x7Fu));
         value >>= 7;
      }

      writer.Write((byte)value);
   }

   /// <summary>Writes a 64-bit integer in a compressed format.</summary>
   /// <param name="writer">The writer to use.</param>
   /// <param name="value">The 64-bit integer to write.</param>
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public static void Write7BitEncodedInt64(this BinaryWriter writer, long value)
   {
#if NET6_0_OR_GREATER
      writer.Write7BitEncodedInt64(value);
#else
      Write7BitEncodedUInt64(writer, (ulong)value);
#endif
   }

   /// <summary>Writes a 64-bit unsigned integer in a compressed format.</summary>
   /// <param name="writer">The writer to use.</param>
   /// <param name="value">The 64-bit unsigned integer to write.</param>
   public static void Write7BitEncodedUInt64(this BinaryWriter writer, ulong value)
   {
      // Note(Nightowl): This implementation was copied from the original as it was only made public in .NET 6.0;

      while (value > 0x7Fu)
      {
         writer.Write((byte)((uint)value | ~0x7Fu));
         value >>= 7;
      }

      writer.Write((byte)value);
   }
   #endregion

   #region Nullable
   /// <summary>
   /// Writes a <see cref="bool"/> value that indicates if the 
   /// given <paramref name="value"/> is <see langword="null"/>.
   /// </summary>
   /// <typeparam name="T">The type of the <paramref name="value"/>.</typeparam>
   /// <param name="writer">The <see cref="BinaryWriter"/> to use.</param>
   /// <param name="value">The value to check.</param>
   /// <returns>
   /// <see langword="false"/> if the <paramref name="value"/> 
   /// is <see langword="null"/>, otherwise <see langword="true"/>.
   /// </returns>
   public static bool TryWriteNullable<T>(this BinaryWriter writer, [NotNullWhen(true)] T? value)
   {
      if (value is null)
      {
         writer.Write(false);
         return false;
      }

      writer.Write(true);
      return true;
   }

   /// <summary>
   /// Writes a <see cref="bool"/> value that indicates if the given <paramref name="value"/> is
   /// <see langword="null"/>. If the <paramref name="value"/> is not <see langword="null"/> then the given
   /// <paramref name="writeCallback"/> is invoked to write the <paramref name="value"/>.
   /// </summary>
   /// <typeparam name="T">The type of the <paramref name="value"/>.</typeparam>
   /// <param name="writer">The <see cref="BinaryWriter"/> to use.</param>
   /// <param name="value">The value to write.</param>
   /// <param name="writeCallback">The callback that is invoked to write the non <see langword="null"/> value.</param>
   public static void TryWriteNullable<T>(this BinaryWriter writer, [NotNullWhen(true)] T? value, Action<T> writeCallback)
   {
      if (TryWriteNullable(writer, value))
         writeCallback.Invoke(value);
   }

   /// <inheritdoc cref="TryWriteNullable{T}(BinaryWriter, T, Action{T})"/>
   public static void TryWriteNullable<T>(this BinaryWriter writer, [NotNullWhen(true)] T? value, Action<BinaryWriter, T> writeCallback)
   {
      if (TryWriteNullable(writer, value))
         writeCallback.Invoke(writer, value);
   }
   #endregion

   #region Methods
   /// <summary>Writes the given <paramref name="guid"/> to the underlying stream.</summary>
   /// <param name="writer">The writer to use.</param>
   /// <param name="guid">The <see cref="Guid"/> to write.</param>
   public static void Write(this BinaryWriter writer, Guid guid)
   {
      byte[] rentedBuffer = ArrayPool<byte>.Shared.Rent(CommonSizes.Guid);
      try
      {
         // rented buffer might be larger
         Span<byte> span = new Span<byte>(rentedBuffer, 0, CommonSizes.Guid);

         guid.TryWriteBytes(span);
         writer.Write(span);
      }
      finally
      {
         ArrayPool<byte>.Shared.Return(rentedBuffer);
      }
   }

   /// <summary>Writes the given <paramref name="timeSpan"/> to the underlying stream.</summary>
   /// <param name="writer">The writer to use.</param>
   /// <param name="timeSpan">The <see cref="TimeSpan"/> write.</param>
   public static void Write(this BinaryWriter writer, TimeSpan timeSpan)
   {
      long ticks = timeSpan.Ticks;
      writer.Write(ticks);
   }

   /// <summary>Writes the given <paramref name="dateTime"/> to the underlying stream.</summary>
   /// <param name="writer">The writer to use.</param>
   /// <param name="dateTime">The <see cref="DateTime"/> to write.</param>
   /// <remarks>This method forces the given <paramref name="dateTime"/> to be in <see cref="DateTimeKind.Utc"/> before writing.</remarks>
   public static void Write(this BinaryWriter writer, DateTime dateTime)
   {
      long ticks = dateTime.ToUniversalTime().Ticks;
      writer.Write(ticks);
   }
   #endregion
}
