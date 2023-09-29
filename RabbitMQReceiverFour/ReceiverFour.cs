using System;
using System.Text;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost");
factory.ClientProvidedName = "Receiver One LinkedIn";


IConnection connection = factory.CreateConnection();

IModel channel = connection.CreateModel();

string exchangeName = "MessagesSpotify";
string routingKey = "SpotifyRouingKey";
string queueName = "QueueSpotify";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey, null);

channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, args) =>
{
    Task.Delay(TimeSpan.FromSeconds(3)).Wait();

    var body = args.Body.ToArray();

    string messageSpotify = Encoding.UTF8.GetString(body);

    Console.WriteLine($"El mensaje recibo por R4 es: {messageSpotify}");

    channel.BasicAck(args.DeliveryTag, false);
};

var consumerTag = channel.BasicConsume(queueName, false, consumer);


// Permite correr el programa hasta que se lean y se cierre el programa
Console.ReadLine();

channel.BasicCancel(consumerTag);

channel.Close();
connection.Close(); 