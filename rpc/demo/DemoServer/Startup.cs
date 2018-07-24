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
using Consul;
using System.Net;

namespace DemoClient
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();

            lifetime.ApplicationStarted.Register(() => {
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
                    Ports = { new ServerPort("0.0.0.0", 12345, ServerCredentials.Insecure) }
                };

                server.Start();

                IConsulClient consul = new ConsulClient(r => {
                    r.Address = new Uri("http://10.10.10.45:8500");
                });

                consul.Agent.ServiceRegister(new AgentServiceRegistration {
                    ID = "1111",
                    Address = "10.10.10.91",
                    Port = 12345,
                    Tags = new[] { "grpc" },
                    Name = "DemoServer",
                    Check = new AgentServiceCheck {
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                        Interval = TimeSpan.FromSeconds(10),
                        Status = HealthStatus.Passing,
                        Timeout = TimeSpan.FromSeconds(5),
                        HTTP = $"http://10.10.10.91:20000/home"
                    }
                }).Wait();
            });
        }
    }
}
