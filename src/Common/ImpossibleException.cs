using System;

namespace TNO.Common;

/// <summary>
/// Represents an <see cref="Exception"/> that should be impossible to obtain.
/// </summary>
[Serializable]
public class ImpossibleException : Exception
{
   #region Consts
   /// <summary>Represents the default exception message that will be used.</summary>
   public const string DefaultMessage = "It should be impossible to get this exception.";
   #endregion

   #region Constructors
   /// <summary>Initializes a new instance of the <see cref="ImpossibleException"/> class with the <see cref="DefaultMessage"/>.</summary>
   /// <inheritdoc/>
   public ImpossibleException() { }

   /// <summary>
   /// Initializes a new instance of the <see cref="ImpossibleException"/>
   /// class with a specified error <paramref name="message"/>.
   /// </summary>
   /// <inheritdoc/>
   public ImpossibleException(string? message) : base(message) { }

   /// <summary>
   /// Initializes a new instance of the <see cref="ImpossibleException"/> class with the <see cref="DefaultMessage"/>
   /// and a reference to the <paramref name="inner"/> exception that is the cause of this exception.
   /// </summary>
   /// <inheritdoc/>
   public ImpossibleException(Exception? inner) : base(DefaultMessage, inner) { }

   /// <summary>
   /// Initializes a new instance of the <see cref="ImpossibleException"/> class with a specified error
   /// <paramref name="message"/> and a reference to the <paramref name="inner"/> exception that is the cause of this exception.
   /// </summary>
   /// <inheritdoc/>
   public ImpossibleException(string? message, Exception? inner) : base(message, inner) { }

   /// <summary>Initializes a new instance of the <see cref="ImpossibleException"/> class with serialized data.</summary>
   /// <inheritdoc/>
   protected ImpossibleException(
    System.Runtime.Serialization.SerializationInfo info,
    System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
   #endregion
}