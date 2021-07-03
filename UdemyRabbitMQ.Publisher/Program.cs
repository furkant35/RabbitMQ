using RabbitMQ.Client;
using System;
using System.Text;

namespace UdemyRabbitMQ.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://inqqdvlv:twaufqgaU6gp-EczlbvBf2XNiqHquq5d@chimpanzee.rmq.cloudamqp.com/inqqdvlv")
            };
            using var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                //channel.QueueDeclare("task_queue", durable: true, false, false, null);
                channel.ExchangeDeclare("logs", durable: true, type: ExchangeType.Fanout);
                string message = GetMessage(args);
                for (int i = 1; i < 11; i++)
                {
                    var bodyByte = Encoding.UTF8.GetBytes($"{message}-{i}");
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    //channel.BasicPublish("", routingKey: "task_queue", properties, body: bodyByte);
                    channel.BasicPublish("logs", routingKey: "", properties, body: bodyByte);
                    Console.WriteLine($"Mesajınız gönderilmiştir:{message}-{i}");
                }
            }
            Console.WriteLine("Çıkış yapmak için tıklayınız");
            Console.ReadLine();
        }
        private static string GetMessage(string[] args)
        {
            return args[0].ToString();
        }
    }
}
