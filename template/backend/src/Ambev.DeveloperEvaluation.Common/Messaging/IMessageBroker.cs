namespace Ambev.DeveloperEvaluation.Common.Messaging;

/// <summary>
/// Defines the contract for a message broker service that can publish messages to a message queue.
/// </summary>
public interface IMessageBroker
{
    /// <summary>
    /// Publishes a message to the specified topic asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the message to publish.</typeparam>
    /// <param name="topic">The topic or routing key to publish the message to.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : class;
} 