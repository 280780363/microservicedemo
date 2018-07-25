using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Data
{
    public class SeedData
    {
        public static List<Client> Clients()
        {
            return new List<Client>
            {
                new Client
                {
                    // 客户端id
                    ClientId = "mvcclient",
                    // 客户端名称
                    ClientName = "Mvc Client",
                    // TOKEN有效时长
                    AccessTokenLifetime = 3600,
                    // 配置TOKEN类型,reference为引用类型,数据不会存在TOKEN中
                    AccessTokenType = AccessTokenType.Jwt,
                    // 配置客户端授权模式
                    AllowedGrantTypes = GrantTypes.Implicit,
                    // 配置客户端连接密码
                    ClientSecrets = {new Secret("123123".Sha256())},
                    // 客户端允许的请求范围
                    AllowedScopes =
                    {
                        "demoapi",
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    //允许离线,即开启refresh_token
                    AllowOfflineAccess = true,
                    RequireClientSecret = false,
                    RedirectUris = {"http://localhost:5001/signin-oidc"}, //跳转登录到的客户端的地址
                    PostLogoutRedirectUris = {"http://localhost:5001/signout-callback-oidc"}, //跳转登出到的客户端的地址
                    RequireConsent = true, //是否需要用户点击确认进行跳转
                    LogoUri =
                        "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1532510629470&di=b437bfeceb9e5d8856276f0236431f83&imgtype=0&src=http%3A%2F%2Fphotocdn.sohu.com%2F20150907%2Fmp30906533_1441629699374_2.jpeg",
                    AllowRememberConsent = true
                },
                new Client
                {
                    // 客户端id
                    ClientId = "spaclient",
                    // 客户端名称
                    ClientName = "Spa Client",
                    // TOKEN有效时长
                    AccessTokenLifetime = 3600,
                    // 配置TOKEN类型,reference为引用类型,数据不会存在TOKEN中
                    AccessTokenType = AccessTokenType.Jwt,
                    // 配置客户端授权模式
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    // 配置客户端连接密码
                    ClientSecrets = {new Secret("123123".Sha256())},
                    // 客户端允许的请求范围
                    AllowedScopes =
                    {
                        "demoapi",
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    //允许离线,即开启refresh_token
                    AllowOfflineAccess = true,
                    RequireClientSecret = false,
                }
            };
        }

        public static IEnumerable<ApiResource> ApiResources()
        {
            return new List<ApiResource>
            {
                // 定义api资源 这里如果使用构造函数传入Name会默认创建一个同名的Scope，
                // 这点需要注意，因为这个Api如果没有Scope，那根本无法访问
                new ApiResource
                {
                    Name = "demoapi",
                    DisplayName = "Demo Api",
                    Description = "测试Api",
                    ApiSecrets = {new Secret("123123".Sha256())},
                    Scopes =
                    {
                        new Scope("demoapi", "Demo Api")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
        }

        public static List<DemoUsers> Users()
        {
            return new List<DemoUsers>
            {
                new DemoUsers
                {
                    UserName = "laowang",
                    Email = "520@qq.com",
                    Id = Guid.NewGuid(),
                    EmailConfirmed = true,
                    TwoFactorEnabled = false,
                    Avatar =
                        "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1528131041794&di=78ae71a3573dc86bc010e301005fea53&imgtype=0&src=http%3A%2F%2Fpic2.orsoon.com%2F2017%2F0309%2F20170309032925886.png"
                },
                new DemoUsers
                {
                    UserName = "zhangsan",
                    Email = "521@qq.com",
                    Id = Guid.NewGuid(),
                    EmailConfirmed = true,
                    TwoFactorEnabled = false,
                    Avatar = "http://pic20.photophoto.cn/20110804/0010023712739303_b.jpg"
                },
                new DemoUsers
                {
                    UserName = "lisi",
                    Email = "522@qq.com",
                    Id = Guid.NewGuid(),
                    EmailConfirmed = true,
                    TwoFactorEnabled = false,
                    Avatar = "http://p1.qzone.la/upload/0/14vy5x96.jpg"
                }
            };
        }
    }
}