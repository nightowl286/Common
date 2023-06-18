using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;

namespace TNO.Common.Serialisation;

/// <summary>
/// Contains useful extension methods related to the <see cref="BinaryReader"/>.
/// </summary>
public static class BinaryReaderExtensions
{
   #region 7 Bit Encoded Integers
   /// <summary>Reads in a 32-bit integer that was written in a compressed format.</summary>
   /// <param name="reader">The reader to use.</param>
   /// <returns>The uncompressed 32-bit integer.</returns>
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public static int Read7BitEncodedInt(this BinaryReader reader)
   {
#if NET6_0_OR_GREATER
      return reader.Read7BitEncodedInt();
#else
      return (int)Read7BitEncodedUInt(reader);
#endif
   }

   /// <summary>Reads in a 32-bit unsigned integer that was written in a compressed format.</summary>
   /// <param name="reader">The reader to use.</param>
   /// <returns>The uncompressed 32-bit unsigned integer.</returns>
   public static uint Read7BitEncodedUInt(this BinaryReader reader)
   {
      // Note(Nightowl): This implementation was copied from the original as it was only made public in .NET 6.0;

      uint result = 0;
      byte byteReadJustNow;

      const int MaxBytesWithoutOverflow = 4;
      for (int shift = 0; shift < MaxBytesWithoutOverflow * 7; shift += 7)
      {
         byteReadJustNow = reader.ReadByte();
         result |= (byteReadJustNow & 0x7Fu) << shift;

         if (byteReadJustNow <= 0x7Fu)
            return result;
      }

      byteReadJustNow = reader.ReadByte();
      if (byteReadJustNow > 0b_1111u)
         throw new InvalidDataException("Too many bytes in what should have been a 7-bit encoded 32-bit integer.");

      result |= (uint)byteReadJustNow << (MaxBytesWithoutOverflow * 7);
      return result;
   }

   /// <summary>Reads in a 64-bit integer that was written in a compressed format.</summary>
   /// <param name="reader">The reader to use.</param>
   /// <returns>The uncompressed 64-bit integer.</returns>
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public static long Read7BitEncodedInt64(this BinaryReader reader)
   {
#if NET6_0_OR_GREATER
      return reader.Read7BitEncodedInt64();
#else
      return (long)Read7BitEncodedUInt64(reader);
#endif
   }

   /// <summary>Reads in a 64-bit unsigned integer that was written in a compressed format.</summary>
   /// <param name="reader">The reader to use.</param>
   /// <returns>The uncompressed 64-bit unsigned integer.</returns>
   public static ulong Read7BitEncodedUInt64(this BinaryReader reader)
   {
      // Note(Nightowl): This implementation was copied from the original as it was only made public in .NET 6.0;

      ulong result = 0;
      byte byteReadJustNow;

      const int MaxBytesWithoutOverflow = 9;
      for (int shift = 0; shift < MaxBytesWithoutOverflow * 7; shift += 7)
      {
         byteReadJustNow = reader.ReadByte();
         result |= (byteReadJustNow & 0x7Ful) << shift;

         if (byteReadJustNow <= 0x7Fu)
            return result;
      }

      byteReadJustNow = reader.ReadByte();
      if (byteReadJustNow > 0b_1u)
         throw new InvalidDataException("Too many bytes in what should have been a 7-bit encoded 64-bit integer.");

      result |= (ulong)byteReadJustNow << (MaxBytesWithoutOverflow * 7);
      return result;
   }
   #endregion

   #region Nullable
   /// <summary>Checks whether the next nullable value in the stream has a value.</summary>
   /// <param name="reader">The reader to use.</param>
   /// <returns>
   /// <see langword="true"/> if the next value can be read, <see langword="false"/> 
   /// if the written value was <see langword="null"/>.
   /// </returns>
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public static bool TryReadNullable(this BinaryReader reader) => reader.ReadBoolean();

   /// <summary>Tries to read a nullable value using the given <paramref name="readCallback"/>.</summary>
   /// <typeparam name="T">The type of the value to read.</typeparam>
   /// <param name="reader">The reader to use.</param>
   /// <param name="readCallback">The callback that will be invoked if the written value was not <see langword="null"/>.</param>
   /// <returns>The value read with the <paramref name="readCallback"/>, or <see langword="null"/>.</returns>
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public static T? TryReadNullable<T>(this BinaryReader reader, Func<T> readCallback)
   {
      if (reader.ReadBoolean())
         return readCallback.Invoke();

      return default;
   }

   /// <summary>Tries to read a nullable value using the given <paramref name="readCallback"/>.</summary>
   /// <typeparam name="T">The type of the value to read.</typeparam>
   /// <param name="reader">The reader to use.</param>
   /// <param name="readCallback">The callback that will be invoked if the written value was not <see langword="null"/>.</param>
   /// <returns>The value read with the <paramref name="readCallback"/>, or <see langword="null"/>.</returns>
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public static T? TryReadNullable<T>(this BinaryReader reader, Func<BinaryReader, T> readCallback)
   {
      if (reader.ReadBoolean())
         return readCallback.Invoke(reader);

      return default;
   }
   #endregion

   #region Methods
   /// <summary>Reads a <see cref="Guid"/> value from the underlying stream.</summary>
   /// <param name="reader">The reader to use.</param>
   /// <returns>The read <see cref="Guid"/> value.</returns>
   /// <exception cref="EndOfStreamException">Thrown if there weren't enough bytes left to read a full <see cref="Guid"/>.</exception>
   public static Guid ReadGuid(this BinaryReader reader)
   {
      byte[] rentedBuffer = ArrayPool<byte>.Shared.Rent(CommonSizes.Guid);
      try
      {
         // rented buffer might be larger
         Span<byte> span = new Span<byte>(rentedBuffer, 0, CommonSizes.Guid);

         int read = reader.Read(span);
         if (read != CommonSizes.Guid)
            throw new EndOfStreamException();

         Guid guid = new Guid(span);
         return guid;
      }
      finally
      {
         ArrayPool<byte>.Shared.Return(rentedBuffer);
      }
   }

   /// <summary>Reads a <see cref="TimeSpan"/> value from the underlying stream.</summary>
   /// <param name="reader">The reader to use.</param>
   /// <returns>The read <see cref="TimeSpan"/> value.</returns>
   public static TimeSpan ReadTimeSpan(this BinaryReader reader)
   {
      long ticks = reader.ReadInt64();
      TimeSpan timeSpan = new TimeSpan(ticks);

      return timeSpan;
   }

   /// <summary>Reads a <see cref="DateTime"/> value from the underlying stream.</summary>
   /// <param name="reader">The reader to use.</param>
   /// <returns>The read <see cref="DateTime"/> value.</returns>
   /// <remarks>This method assumes that the written <see cref="DateTime"/> was in <see cref="DateTimeKind.Utc"/>.</remarks>
   public static DateTime ReadDateTime(this BinaryReader reader)
   {
      long ticks = reader.ReadInt64();
      DateTime dateTime = new DateTime(ticks, DateTimeKind.Utc);

      return dateTime;
   }
   #endregion
}
