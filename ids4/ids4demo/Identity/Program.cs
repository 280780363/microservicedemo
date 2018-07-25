using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Identity.Data;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Identity.Infrastructure;
namespace Identity
{
    public class Program
    {
        public static void Main(string[] args) {
            CreateWebHostBuilder(args).Build()
            .Migration<DemoIdentityDbContext>((context, service) => {
                // 注入用户管理 增加用户
                var userManager = service.GetRequiredService<UserManager<DemoUsers>>();
                foreach (var user in SeedData.Users()) {
                    if (userManager.FindByNameAsync(user.UserName).Result == null) {
                        userManager.CreateAsync(user, "123123").Wait();
                    }
                }
            })
            .Migration<PersistedGrantDbContext>(null)
            .Migration<ConfigurationDbContext>((context, service) => {
                if (!context.ApiResources.Any())
                    context.ApiResources.AddRange(SeedData.ApiResources().Select(r => r.ToEntity()));
                if (!context.IdentityResources.Any())
                    context.IdentityResources.AddRange(SeedData.IdentityResources().Select(r => r.ToEntity()));
                if (!context.Clients.Any())
                    context.Clients.AddRange(SeedData.Clients().Select(r => r.ToEntity()));
            })
            .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseUrls("http://*:5000")
                .UseStartup<Startup>();


    }
}
