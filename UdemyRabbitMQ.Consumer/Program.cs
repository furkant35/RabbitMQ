using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace UdemyRabbitMQ.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://inqqdvlv:twaufqgaU6gp-EczlbvBf2XNiqHquq5d@chimpanzee.rmq.cloudamqp.com/inqqdvlv")
            };
            // factory.HostName = "localhost"; 
            using var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                //channel.QueueDeclare("task_queue", durable: true, false, false, null);
                channel.ExchangeDeclare("logs", durable: true, type: ExchangeType.Fanout);
                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName, exchange: "logs", routingKey: "");
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, false);
                //Console.WriteLine("mesajları bekliyorum..");
                Console.WriteLine("logları bekliyorum..");
                var consumer = new EventingBasicConsumer(channel);



                //channel.BasicConsume("task_queue", autoAck: false, consumer);
                channel.BasicConsume(queueName, false, consumer);


                consumer.Received += (model, ea) =>
                {
                    //var message = ea.Body;
                    var log = ea.Body;
                    //Console.WriteLine("Mesaj alındı: " + message);
                    Console.WriteLine("Log alındı: " + log);
                    int time = int.Parse(GetMessage(args));
                    Thread.Sleep(time);
                    //Console.WriteLine("Mesaj işlendi..");
                    Console.WriteLine("Loglama bitti..");
                    channel.BasicAck(ea.DeliveryTag, false);
                };
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
