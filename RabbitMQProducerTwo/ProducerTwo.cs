using System;
using System.Text;
using RabbitMQ;
using RabbitMQ.Client;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost");
factory.ClientProvidedName = "ProducerSpotify";


IConnection connection = factory.CreateConnection();

IModel channel = connection.CreateModel();

string exchangeName = "MessagesSpotify";
string routingKey = "SpotifyRouingKey";
string queueName = "QueueSpotify";


channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey, null);

for (int i = 0; i < 60; i++)
{
    Console.WriteLine($"Sending Message: {i}");
    byte[] messageLink = Encoding.UTF8.GetBytes($"Message Spotify: {i}");
    channel.BasicPublish(exchangeName, routingKey, null, messageLink);
    Thread.Sleep(1000);

}

channel.Close();
connection.Close();
