namespace TNO.Common.Serialisation;

/// <summary>
/// Contains the sizes (in bytes) of common structures.
/// </summary>
public static class CommonSizes
{
   #region Constants
   /// <summary>The size of a <see cref="System.Guid"/> in bytes.</summary>
   public const int Guid = 16;

   /// <summary>The size of a <see cref="System.TimeSpan"/> in bytes.</summary>
   public const int TimeSpan = sizeof(long);

   /// <summary>The size of a <see cref="System.DateTime"/> in bytes.</summary>
   public const int DateTime = sizeof(long);
   #endregion
}
