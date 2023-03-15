namespace TNO.Common.Extensions;

// Todo(Nightowl): Ignoring documentation for this, need to figure out why it even exists;
public static class ByteArrayExtensions
{
   #region Functions
   public static bool AreSame(this byte[] arr, byte[] otherArr)
   {
      if (arr == otherArr) return true;
      if (arr == null || otherArr == null) return false;

      if (otherArr.Length != arr.Length) return false;

      for (int i = 0; i < arr.Length; i++)
      {
         if (arr[i] != otherArr[i])
            return false;
      }

      return true;
   }
   #endregion
}
