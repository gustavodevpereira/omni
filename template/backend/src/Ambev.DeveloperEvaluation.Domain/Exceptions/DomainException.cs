using System;

namespace Ambev.DeveloperEvaluation.Domain.Exceptions;

/// <summary>
/// Represents an exception that occurs when a domain business rule is violated.
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DomainException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainException"/> class with a specified entity name and error details.
    /// </summary>
    /// <param name="entityName">The name of the entity where the error occurred.</param>
    /// <param name="errorDetails">Details about the business rule violation.</param>
    public DomainException(string entityName, string errorDetails) 
        : base($"A business rule was violated in entity '{entityName}': {errorDetails}")
    {
        EntityName = entityName;
        ErrorDetails = errorDetails;
    }

    /// <summary>
    /// Gets the name of the entity where the error occurred.
    /// </summary>
    public string? EntityName { get; }

    /// <summary>
    /// Gets details about the business rule violation.
    /// </summary>
    public string? ErrorDetails { get; }
}
