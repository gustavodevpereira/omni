using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Common.Messaging;

/// <summary>
/// Simple implementation of the IMessageBroker interface that logs messages to the console.
/// This is a temporary placeholder for the Rebus implementation.
/// 
/// NOTE: This should be replaced with a proper Rebus implementation in a production environment.
/// The Rebus implementation would use RabbitMQ as the transport and properly handle message routing.
/// 
/// Example Rebus configuration:
/// ```csharp
/// services.AddRebus(configure => configure
///     .Transport(t => t.UseRabbitMq("amqp://guest:guest@localhost:5672", "ambev_events"))
///     .Routing(r => r.TypeBased())
///     .Options(o => {
///         o.SetNumberOfWorkers(1);
///         o.SetMaxParallelism(1);
///     }));
/// ```
/// </summary>
public class SimpleMessageBroker : IMessageBroker
{
    private readonly ILogger<SimpleMessageBroker> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleMessageBroker"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public SimpleMessageBroker(ILogger<SimpleMessageBroker> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Simulates publishing a message to a topic by logging it.
    /// In a Rebus implementation, this would send the message to the actual message broker.
    /// </summary>
    /// <typeparam name="T">The type of the message to publish.</typeparam>
    /// <param name="topic">The topic to publish the message to.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            _logger.LogInformation(
                "[REBUS SIMULATION] Publishing message to topic {Topic}: {MessageType} - {MessageContent}", 
                topic,
                typeof(T).Name,
                JsonSerializer.Serialize(message, new JsonSerializerOptions { WriteIndented = true }));

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish message of type {MessageType} to topic {Topic}", typeof(T).Name, topic);
            throw;
        }
    }
} 