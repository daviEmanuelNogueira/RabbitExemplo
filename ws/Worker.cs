using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ws.Data;
using ws.Models;

namespace ws;

public class Worker(IServiceScopeFactory scopeFactory) : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = "rbq", Port = 5672 };
        // var factory = new ConnectionFactory { HostName = "localhost" };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(queue: "fila", durable: true, exclusive: false, autoDelete: false, arguments: null);
        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

        Console.WriteLine(" [*] Connected to RabbitMQ");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var jsonString = Encoding.UTF8.GetString(body);
                
                if (string.IsNullOrEmpty(jsonString))
                    throw new Exception("Empty message");

                Console.WriteLine(" [x] Received:", jsonString);
                
                var message = JsonSerializer.Deserialize<Notification>(jsonString) ?? throw new Exception("Invalid message");

                // Cria escopo para o DbContext
                using var scope = _scopeFactory.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await ctx.Notifications!.AddAsync(message);
                await ctx.SaveChangesAsync();

                // Simula um processamento de 20 segundos
                await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);

                // Confirmação manual da mensagem
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                Console.WriteLine(" [x] Message acknowledged");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" [!] Error processing message: {ex.Message}");
                // Rejeita a mensagem e reencaminha para a fila (requeue: true)
                await channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await channel.BasicConsumeAsync(queue: "fila", autoAck: false, consumer: consumer);

        // Mantenha o serviço ativo
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
