using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args) {
            //define the cluster configuration
            var builder = new SiloHostBuilder()
                //configure the cluster with local host clustering
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options => {
                    options.ClusterId = "dev";
                    options.ServiceId = "HelloWorldApp";
                })
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureApplicationParts(r => {
                    r.AddApplicationPart(typeof(IUsers).Assembly).WithReferences();
                })
                .ConfigureLogging(logging => logging.AddConsole());
            //build the silo
            var host = builder.Build();
            //start the silo
            host.StartAsync().Wait();

            CreateWebHostBuilder(args).Build().Run();


        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
