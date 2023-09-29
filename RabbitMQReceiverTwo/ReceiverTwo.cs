using System;
using System.Text;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost");
factory.ClientProvidedName = "Receiver Two LinkedIn";


IConnection connection = factory.CreateConnection();

IModel channel = connection.CreateModel();

string exchangeName = "MessagesLinkedIn";
string routingKey = "linkedinRouteKey";
string queueName = "QueueLinkedIn";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey, null);

channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, args) =>
{
    Task.Delay(TimeSpan.FromSeconds(3)).Wait();

    var body = args.Body.ToArray();

    string messageLinkedIn = Encoding.UTF8.GetString(body);

    Console.WriteLine($"El mensaje recibo por R2 es: {messageLinkedIn}");

    channel.BasicAck(args.DeliveryTag, false);
};

var consumerTag = channel.BasicConsume(queueName, false, consumer);

Console.ReadLine();
channel.BasicCancel(consumerTag);

channel.Close();
connection.Close();

