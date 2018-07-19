using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MagicOnion;
using MagicOnion.Server;
using Grpc.Core;

namespace DemoClient
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            var service = MagicOnionEngine.BuildServerServiceDefinition(new[] { typeof(Startup).Assembly }, new MagicOnionOptions(true) {
                // MagicOnionLogger = new MagicOnionLogToGrpcLogger(),
                MagicOnionLogger = new MagicOnionLogToGrpcLoggerWithNamedDataDump(),
                GlobalFilters = new MagicOnionFilterAttribute[]
                {
                },
                EnableCurrentContext = true
            });

            var server = new global::Grpc.Core.Server {
                Services = { service },
                Ports = { new ServerPort("localhost", 12345, ServerCredentials.Insecure) }
            };

            server.Start();

            //service.AddSwaggerGen
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) => {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
