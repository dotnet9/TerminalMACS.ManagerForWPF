using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory();
factory.HostName = "localhost";
factory.DispatchConsumersAsync = true;
const string exchangeName = "exchange1";
const string eventName = "myEvent";
using var conn = factory.CreateConnection();
while (true)
{
    var msg = DateTime.Now.TimeOfDay.ToString();
    using (var channel = conn.CreateModel())
    {
        var properties = channel.CreateBasicProperties();
        properties.DeliveryMode = 2;
        channel.ExchangeDeclare(exchangeName, "direct");
        var body = Encoding.UTF8.GetBytes(msg);
        channel.BasicPublish(exchangeName, eventName, true, properties, body);
    }

    Console.WriteLine($"发布了消息：{msg}");
    Thread.Sleep(TimeSpan.FromSeconds(1));
}