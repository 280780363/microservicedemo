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
            var user = await service.Get(1);
            Console.WriteLine(user.Name);
            Console.ReadLine();
        }
    }
}
