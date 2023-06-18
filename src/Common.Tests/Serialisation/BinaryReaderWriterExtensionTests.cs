using System.Text;
using TNO.Tests.Common;
using Reading = TNO.Common.Serialisation.BinaryReaderExtensions;
using Writing = TNO.Common.Serialisation.BinaryWriterExtensions;

namespace TNO.Common.Tests.Serialisation;

[TestClass]
[TestCategory(Category.Serialisation)]
public class BinaryReaderWriterExtensionTests
{
   #region Tests
   [TestMethod]
   public void ReadWrite_Guid_Successful()
   {
      // Arrange
      Guid expected = Guid.NewGuid();

      // Act
      Guid result = WriteAndRead(expected, Writing.Write, Reading.ReadGuid);

      // Assert
      Assert.That.AreEqual(expected, result);
   }

   [TestMethod]
   public void ReadWrite_TimeSpan_MinValue_Successful()
   {
      // Arrange
      TimeSpan expected = TimeSpan.MinValue;

      // Act
      TimeSpan result = WriteAndRead(expected, Writing.Write, Reading.ReadTimeSpan);

      // Assert
      Assert.That.AreEqual(expected, result);
   }

   [TestMethod]
   public void ReadWrite_TimeSpan_MaxValue_Successful()
   {
      // Arrange
      TimeSpan expected = TimeSpan.MaxValue;

      // Act
      TimeSpan result = WriteAndRead(expected, Writing.Write, Reading.ReadTimeSpan);

      // Assert
      Assert.That.AreEqual(expected, result);
   }

   [TestMethod]
   public void ReadWrite_DateTime_MinValue_Successful()
   {
      // Arrange
      DateTime expected = DateTime.MinValue.ToUniversalTime();

      // Act
      DateTime result = WriteAndRead(expected, Writing.Write, Reading.ReadDateTime);

      // Assert
      Assert.That.AreEqual(expected, result);
   }

   [TestMethod]
   public void ReadWrite_DateTime_MaxValue_Successful()
   {
      // Arrange
      DateTime expected = DateTime.MaxValue.ToUniversalTime();

      // Act
      DateTime result = WriteAndRead(expected, Writing.Write, Reading.ReadDateTime);

      // Assert
      Assert.That.AreEqual(expected, result);
   }

   [DataRow(int.MinValue, DisplayName = "Minimum")]
   [DataRow(int.MaxValue, DisplayName = "Maximum")]
   [TestMethod]
   public void ReadWrite_7BitEncodedInt_Successful(int expected)
   {
      // Act
      int result = WriteAndRead(expected, Writing.Write7BitEncodedInt, Reading.Read7BitEncodedInt);

      // Assert
      Assert.That.AreEqual(expected, result);
   }

   [DataRow(uint.MinValue, DisplayName = "Minimum")]
   [DataRow(uint.MaxValue, DisplayName = "Maximum")]
   [TestMethod]
   public void ReadWrite_7BitEncodedUInt_Successful(uint expected)
   {
      // Act
      uint result = WriteAndRead(expected, Writing.Write7BitEncodedUInt, Reading.Read7BitEncodedUInt);

      // Assert
      Assert.That.AreEqual(expected, result);
   }

   [DataRow(long.MinValue, DisplayName = "Minimum")]
   [DataRow(long.MaxValue, DisplayName = "Maximum")]
   [TestMethod]
   public void ReadWrite_7BitEncodedInt64_Successful(long expected)
   {
      // Act
      long result = WriteAndRead(expected, Writing.Write7BitEncodedInt64, Reading.Read7BitEncodedInt64);

      // Assert
      Assert.That.AreEqual(expected, result);
   }

   [DataRow(ulong.MinValue, DisplayName = "Minimum")]
   [DataRow(ulong.MaxValue, DisplayName = "Maximum")]
   [TestMethod]
   public void ReadWrite_7BitEncodedUInt64_Successful(ulong expected)
   {
      // Act
      ulong result = WriteAndRead(expected, Writing.Write7BitEncodedUInt64, Reading.Read7BitEncodedUInt64);

      // Assert
      Assert.That.AreEqual(expected, result);
   }
   #endregion

   #region Helpers
   private static T WriteAndRead<T>(T value, Action<BinaryWriter, T> writeCallback, Func<BinaryReader, T> readCallback, Encoding? encoding = null)
   {
      encoding ??= Encoding.UTF8;

      using (MemoryStream memoryStream = new MemoryStream())
      {
         using (BinaryWriter writer = new BinaryWriter(memoryStream, encoding, true))
            writeCallback.Invoke(writer, value);

         memoryStream.Position = 0;

         using (BinaryReader reader = new BinaryReader(memoryStream, encoding))
            return readCallback.Invoke(reader);
      }
   }
   #endregion
}
