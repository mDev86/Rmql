using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rmql;
using Rmql.Enums;

namespace DemoApp.Workers
{
    public class TestWorker: RmqlWorker<MqMessage>
    {
        public override string Key => "TestQueue";
        public override int Frequency => 50;
        public override RedisDbEnum RedisDb => RedisDbEnum.Db0;

        public override Task ActionAsync(IServiceScope serviceScope, MqMessage message)
        {
            Console.WriteLine($"Handling message from {Key}: {message.Id}; {message.Message}");
            return Task.CompletedTask;
        }
    }
}