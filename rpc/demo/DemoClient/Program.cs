using Grpc.Core;
using MagicOnion.Client;
using System;
using System.Threading.Tasks;

namespace DemoClient
{
    class Program
    {
        static void Main(string[] args) {
            Test().Wait();
        }

        public static async Task Test() {
            var serverChannel = new Channel("localhost", 12345, ChannelCredentials.Insecure);
            //var context = new ChannelContext(serverChannel);

            var service = MagicOnionClient.Create<IUsers>(serverChannel);

            DateTime d1 = DateTime.Now;
            for (int i = 0; i < 10000; i++) {
                var user = await service.GetAll();
            }
            Console.WriteLine($"执行1w次总计耗时：{(DateTime.Now - d1).TotalSeconds}");


            DateTime d2 = DateTime.Now;
            for (int i = 0; i < 10000; i++) {
                var user = await service.Get(1);
            }
            Console.WriteLine($"执行1w次总计耗时：{(DateTime.Now - d2).TotalSeconds}");

            DateTime d3 = DateTime.Now;
            for (int i = 0; i < 10000; i++) {
                var user = await service.GetAll();
            }
            Console.WriteLine($"执行1w次总计耗时：{(DateTime.Now - d3).TotalSeconds}");

            DateTime d4 = DateTime.Now;
            for (int i = 0; i < 10000; i++) {
                var user = await service.GetAll();
            }
            Console.WriteLine($"执行1w次总计耗时：{(DateTime.Now - d4).TotalSeconds}");
        }
    }
}
