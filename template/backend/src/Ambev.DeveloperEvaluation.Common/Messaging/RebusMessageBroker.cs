using Microsoft.Extensions.Logging;
using Rebus.Bus;

namespace Ambev.DeveloperEvaluation.Common.Messaging;

/// <summary>
/// Implementation of IMessageBroker using Rebus.
/// This broker sends messages directly to the RabbitMQ queue.
/// </summary>
public class RebusMessageBroker : IMessageBroker
{
    private readonly IBus _bus;
    private readonly ILogger<RebusMessageBroker> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RebusMessageBroker"/> class.
    /// </summary>
    /// <param name="bus">The Rebus bus instance.</param>
    /// <param name="logger">The logger instance.</param>
    public RebusMessageBroker(IBus bus, ILogger<RebusMessageBroker> logger)
    {
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Publishes a message to RabbitMQ using Rebus.
    /// The message is sent directly to the configured queue with the topic included in the message headers.
    /// </summary>
    /// <typeparam name="T">The type of message to publish.</typeparam>
    /// <param name="topic">The topic to publish to.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            _logger.LogInformation("Publishing message of type {MessageType} with topic {Topic}", typeof(T).Name, topic);
            
            // Add the topic as a message header for reference and filtering in RabbitMQ Management
            var headers = new Dictionary<string, string>
            {
                ["x-custom-topic"] = topic
            };
            
            // Use Send instead of Publish - this sends directly to the configured queue
            await _bus.Advanced.Routing.Send("ambev_events", message, headers);
            
            _logger.LogInformation("Successfully sent message of type {MessageType} with topic {Topic}", typeof(T).Name, topic);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message of type {MessageType} with topic {Topic}: {Message}", 
                typeof(T).Name, topic, ex.Message);
            throw;
        }
    }
} 