using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ws.Data;
using ws.Models;

namespace ws;

public class Worker : BackgroundService
{
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
                
                Console.WriteLine(" [x] Received {0}", jsonString);
                
                var message = JsonSerializer.Deserialize<Notification>(jsonString);
                var db = new AppDbContext();
                db.Notifications!.Add(message);
                await db.SaveChangesAsync();
                // Simula um processamento de 20 segundos
                await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);

                // Confirmação manual da mensagem
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                Console.WriteLine(" [x] Message acknowledged");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" [!] Error processing message: {ex.Message}");
            }
        };

        await channel.BasicConsumeAsync(queue: "fila", autoAck: false, consumer: consumer);

        // Mantenha o serviço ativo
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
