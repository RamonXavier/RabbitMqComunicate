using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// Configurações de conexão com o RabbitMQ

var factory = new ConnectionFactory
{
    HostName = "localhost", // Use "localhost" para o host
    Port = 5672,
    UserName = "guest",
    Password = "guest",
    VirtualHost = "guiando",
    ClientProvidedName = "teste",
};

// Criar uma conexão com o RabbitMQ
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
const string queueName = "fila_teste";

channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Mensagem recebida: {message}");
};

// Registre o consumidor na fila
channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

Console.WriteLine("Aperte qualquer tecla para encerrar o consumer");
Console.ReadLine();