using System;
using System.Text;
using RabbitMQ.Client;

namespace Publisher
{
    class Program
    {

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:13377/"),
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "starter.queue",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);


            bool running = true;
            int option = 0;
            while (running)
            {
                Console.WriteLine(@"
                Choose an option:
                1 - Publish a message
                9 - Exit");

                if (!int.TryParse(Console.ReadLine(), out option))
                {
                    Console.WriteLine("Invalid option\n");
                    continue;
                }

                switch (option)
                {
                    case 1:
                        Console.WriteLine("Enter a message to publish:");
                        var message = Console.ReadLine();
                        BasicPublish(channel, message);
                        break;
                    case 9:
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }

            }
        }

        private static void BasicPublish(IModel channel, string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "starter.queue",
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine($" [x] Sent {message}");
        }
    }
}
