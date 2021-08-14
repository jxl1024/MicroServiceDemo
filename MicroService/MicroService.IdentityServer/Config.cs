using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MicroService.IdentityServer
{
    public class Config
    {
        public static IEnumerable<ApiScope> ApiScopes => new[]
       {
           new ApiScope
           {
               Name="TeamService",
               DisplayName="TeamService api需要被保护"
           }
        };

        public static IEnumerable<Client> Clients => new[]
        {
           // 客户端许可凭据
           new Client
           {
               ClientId = "simple_client",
               // 一个id可以对应多个秘钥
               ClientSecrets =
               {
                   // Sha256加密
                   new Secret("simple_client_secret".Sha256())
               },
               // 指定客户端授权类型  这里是客户端凭据许可模式
               AllowedGrantTypes = GrantTypes.ClientCredentials,
               // 指定API允许的范围  是个数组，可以定义多组
               // 表示当前客户端，允许访问simple_api范围里的API
               AllowedScopes={ "TeamService" }
           },
           // 资源拥有者凭据(用户名和密码模式)
           new Client
           {
               ClientId = "simple_pass_client",
               // 一个id可以对应多个秘钥
               ClientSecrets =
               {
                   // Sha256加密
                   new Secret("simple_client_secret".Sha256())
               },
               // 指定客户端授权类型  这里是资源拥有者凭据许可模式
               AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
               // 指定API允许的范围  是个数组，可以定义多组
               // 表示当前客户端，允许访问simple_api范围里的API
               AllowedScopes={ "TeamService" }
           },
           // openid客户
           new Client
           {
                ClientId="simple_code",
                ClientSecrets={new Secret("simple_code_secret".Sha256())},
                AllowedGrantTypes=GrantTypes.Code,
                RequireConsent=false,
                RequirePkce=true,
                // 重定向  客户端地址 这里的地址是写死的 方便测试
                RedirectUris={ "https://localhost:7001/signin-oidc"},
                //  登录退出地址 
                PostLogoutRedirectUris={ "https://localhost:7001/signout-callback-oidc"},
                
                AllowedScopes=new List<string>{
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "TeamService" // 启用对刷新令牌的支持
                },

                // 增加授权访问
                AllowOfflineAccess=true
           }
        };

        /// <summary>
        /// 在config中添加openid身份资源声明
        /// </summary>
        public static IEnumerable<IdentityResource> Ids => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

        /// <summary>
        /// 创建测试用户
        /// TestUser是Identity Server 4里面的
        /// </summary>
        public static List<TestUser> Users => new List<TestUser>
        {
           new TestUser
           {
               SubjectId="1",
               Username="admin",
               Password="123"
           },
           // openid 身份验证
           new TestUser{SubjectId = "2", Username = "test", Password = "123456",
               Claims =
               {
                   new Claim(JwtClaimTypes.Name, "test"),
                   new Claim(JwtClaimTypes.GivenName, "test"),
                   new Claim(JwtClaimTypes.FamilyName, "test"),
                   new Claim(JwtClaimTypes.Email, "test@email.com"),
                   new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                   new Claim(JwtClaimTypes.WebSite, "http://admin.com"),
                 //  new Claim(JwtClaimTypes.Address, @"{ '城市': '杭州', '邮政编码': '310000' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
               }
           }
        };
    }
}
