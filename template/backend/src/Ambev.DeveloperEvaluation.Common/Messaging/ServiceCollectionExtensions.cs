using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Routing;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.Common.Messaging;

/// <summary>
/// Extension methods for configuring messaging services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Rebus message broker to the service collection.
    /// Configures Rebus to use RabbitMQ as the transport with direct routing to the configured queue.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The service collection with the message broker configured.</returns>
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
    {
        // Get RabbitMQ connection string from configuration or use default with correct credentials
        var rabbitMqConnectionString = configuration.GetConnectionString("RabbitMQ") 
            ?? "amqp://developer:ev%40luAt10n@localhost:50731";
        
        // Get queue name from configuration or use default
        var queueName = configuration["MessageBroker:QueueName"] ?? "ambev_events";

        // Log the connection details for debugging
        Console.WriteLine($"Connecting to RabbitMQ with: {rabbitMqConnectionString}, Queue: {queueName}");

        try
        {
            // Configure and register Rebus
            services.AddRebus(configure => configure
                .Transport(t => t.UseRabbitMq(rabbitMqConnectionString, queueName))
                .Routing(r => {
                    // Configure direct mapping - all messages go to the ambev_events queue
                    // regardless of topic/type
                    r.TypeBased()
                      // Map common message types that likely exist in the system
                      .MapFallback(queueName); // If no other mapping works, use this one
                })
                .Options(o => {
                    o.SetNumberOfWorkers(0); // Disable workers to avoid consuming messages
                    o.SetMaxParallelism(1);  // Must be a positive value
                    o.LogPipeline(verbose: true);
                }),
                isDefaultBus: true, 
                startAutomatically: true);

            // Important: Do not register automatic handlers
            // This would normally be done with services.AutoRegisterHandlersFromAssembly(...)
            // But we deliberately avoid this to keep messages in the queue
        }
        catch (Exception ex)
        {
            // Log any exception during configuration
            Console.WriteLine($"Error configuring Rebus: {ex.Message}");
            throw;
        }

        // Register our Rebus message broker implementation
        services.AddTransient<IMessageBroker, RebusMessageBroker>();

        return services;
    }
} 