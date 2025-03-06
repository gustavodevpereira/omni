namespace Ambev.DeveloperEvaluation.Application.Common.Exceptions;

/// <summary>
/// Exception thrown when a requested entity could not be found.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the NotFoundException class.
    /// </summary>
    public NotFoundException() : base("The requested resource was not found.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the NotFoundException class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public NotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the NotFoundException class with a specified error message and inner exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the NotFoundException class with a name and key to customize the error message.
    /// </summary>
    /// <param name="name">The name of the entity type that was not found.</param>
    /// <param name="key">The key value that was used to search for the entity.</param>
    public NotFoundException(string name, object key)
        : base($"Entity '{name}' with key '{key}' was not found.")
    {
    }
} 