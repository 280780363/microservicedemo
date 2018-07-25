using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Identity.Data;
using Identity.Infrastructure;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity
{
    public class Startup
    {
        IConfiguration configuration;
        public Startup(IConfiguration configuration) {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            var connStr = configuration.GetConnectionString("default");
            var assemblyName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            // 配置AspNetCore Identity 的DbContext服务
            services.AddDbContext<DemoIdentityDbContext>(r => {
                r.UseNpgsql(connStr, options => {
                    // 配置迁移时程序集
                    options.MigrationsAssembly(assemblyName);
                });
            });

            // 配置AspNetCore Identity服务用户密码的验证规则
            services.AddIdentity<DemoUsers, DemoRoles>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

            })
            // 告诉AspNetCore Identity 使用DemoDbContext为数据库上下文
            .AddEntityFrameworkStores<DemoIdentityDbContext>()
            .AddDefaultTokenProviders();

            // identityserver4
            services.AddIdentityServer()
                .AddSigningCredential(GetCredential())
                .AddAspNetIdentity<DemoUsers>()
                .AddConfigurationStore(options => {
                    options.ConfigureDbContext = builder => {
                        builder.UseNpgsql(connStr, sql => {
                            sql.MigrationsAssembly(assemblyName);
                        });
                    };
                })
                .AddOperationalStore(options => {
                    options.ConfigureDbContext = builder => {
                        builder.UseNpgsql(connStr, sql => {
                            sql.MigrationsAssembly(assemblyName);
                        });
                    };
                });
            services.AddTransient<IProfileService, ProfileService>();
            // 配置跨域，允许所有
            services.AddCors(r => {
                r.AddPolicy("all", policy => {
                    policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    ;
                });
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("all");
            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }

        private X509Certificate2 GetCredential() {
            //var sm = typeof(Startup).Assembly.GetManifestResourceStream("Identity.sign.pfx");
            //byte[] data = new byte[sm.Length];
            //sm.Read(data, 0, (int)sm.Length);
            //return new X509Certificate2(data, "123123");

            return new X509Certificate2("sign.pfx", "123123");
        }
    }
}
