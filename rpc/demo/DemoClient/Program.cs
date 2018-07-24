using Consul;
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
            IConsulClient consul = new ConsulClient(r => {
                r.Address = new Uri("http://10.10.10.45:8500");
            });
            var services = await consul.Catalog.Service("DemoServer");
            var list = services.Response;
            var address = list[0].ServiceAddress;
            var port = list[0].ServicePort;

            DateTime d0 = DateTime.Now;
            for (int i = 0; i < 10000; i++) {
                var services0 = await consul.Catalog.Service("DemoServer");
                var list0 = services0.Response;
                var address0 = list0[0].ServiceAddress;
                var port0 = list0[0].ServicePort;
            }
            Console.WriteLine($"consul 1w次获取耗时：{(DateTime.Now - d0).TotalSeconds}");

            var serverChannel = new Channel(address, port, ChannelCredentials.Insecure);
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
