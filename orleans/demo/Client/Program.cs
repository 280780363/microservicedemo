using Models;
using Orleans;
using Orleans.Configuration;
using System;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args) {

            Run().Wait();
        }

        public static async Task Run() {
            var client = new ClientBuilder()
                    .UseLocalhostClustering()
                    .Configure<ClusterOptions>(options => {
                        options.ClusterId = "dev";
                        options.ServiceId = "HelloWorldApp";
                    })
                    .Build();

            //connect the client to the cluster, in this case, which only contains one silo
            await client.Connect();
            // example of calling grains from the initialized client
            var users = client.GetGrain<IUsers>(0);

            DateTime d1 = DateTime.Now;
            for (int i = 0; i < 10000; i++) {
                var uuu = await users.All();
            }
            Console.WriteLine($"执行1w次总计耗时：{(DateTime.Now - d1).TotalSeconds}秒");

            DateTime d2 = DateTime.Now;
            for (int i = 0; i < 10000; i++) {
                var uuu = await users.All();
            }
            Console.WriteLine($"执行1w次总计耗时：{(DateTime.Now - d2).TotalSeconds}秒");

            DateTime d3 = DateTime.Now;
            for (int i = 0; i < 10000; i++) {
                var uuu = await users.All();
            }
            Console.WriteLine($"执行1w次总计耗时：{(DateTime.Now - d3).TotalSeconds}秒");

            DateTime d4 = DateTime.Now;
            for (int i = 0; i < 10000; i++) {
                var uuu = await users.All();
            }
            Console.WriteLine($"执行1w次总计耗时：{(DateTime.Now - d4).TotalSeconds}秒");
        }
    }
}
