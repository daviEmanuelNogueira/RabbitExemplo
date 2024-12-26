using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/notification", async (Notification notification) =>
{
    var factory = new ConnectionFactory { HostName = "rbq", Port = 5672 };
    using var connection = await factory.CreateConnectionAsync();
    using var channel = await connection.CreateChannelAsync();

    await channel.QueueDeclareAsync(queue: "fila", durable: true, exclusive: false, autoDelete: false,
        arguments: null);

    var message = JsonSerializer.Serialize(notification);
    var body = Encoding.UTF8.GetBytes(message);

    await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "fila", body: body);

    return Results.Created("/notifications", notification);
});

app.Run();

internal record Notification(string Message, DateTime Date);
