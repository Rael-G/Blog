namespace Blog.Domain;

/// <summary>
/// Represents errors that occur during application execution within the blog domain.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DomainException"/> class with a specified error message.
/// </remarks>
/// <param name="message">The message that describes the error.</param>
public class DomainException(string message) : Exception(message)
{
}