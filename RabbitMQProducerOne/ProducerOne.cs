using System;
using System.Text;
using RabbitMQ;
using RabbitMQ.Client;


ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost");
factory.ClientProvidedName = "Producer LinkedIn";   


IConnection connection = factory.CreateConnection();
IModel channel = connection.CreateModel();

string exchangeName = "MessagesLinkedIn";
string routingKey = "linkedinRouteKey";
string queueName = "QueueLinkedIn";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey, null);

for (int i = 0; i < 60; i++)
{
    Console.WriteLine($"Sending Message: {i}");
    byte[] messageLink = Encoding.UTF8.GetBytes($"Message LinkedIn: {i}");
    channel.BasicPublish(exchangeName, routingKey, null, messageLink);
    Thread.Sleep(1000);

}


channel.Close();
connection.Close();