using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();
factory.HostName = "localhost";
factory.DispatchConsumersAsync = true;
const string exchangeName = "exchange1";
const string eventName = "myEvent";
using var conn = factory.CreateConnection();
using var channel = conn.CreateModel();
var queueName = "queue1";
channel.ExchangeDeclare(exchangeName, "direct");
channel.QueueDeclare(queueName, true, false, false, null);
channel.QueueBind(queueName, exchangeName, eventName);
var consumer = new AsyncEventingBasicConsumer(channel);
consumer.Received += Consumer_Received;
channel.BasicConsume(queueName, false, consumer);
Console.ReadLine();

async Task Consumer_Received(object sender, BasicDeliverEventArgs args)
{
    try
    {
        var bytes = args.Body.ToArray();
        var msg = Encoding.UTF8.GetString(bytes);
        Console.WriteLine($"{DateTime.Now}收到了消息{msg}");
        channel.BasicAck(args.DeliveryTag, false);
        await Task.Delay(TimeSpan.FromMilliseconds(800));
    }
    catch (Exception ex)
    {
        channel.BasicReject(args.DeliveryTag, true);
        Console.WriteLine($"处理收到的消息出错{ex}");
    }
}