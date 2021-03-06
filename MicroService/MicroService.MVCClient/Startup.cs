using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.MVCClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 1、添加身份验证
            // 我们使用cookie来本地登录用户（通过“Cookies”作为DefaultScheme），并且将DefaultChallengeScheme设置为oidc。
            // 因为当我们需要用户登录时，我们将使用OpenID Connect协议。
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc"; // openid connect
            })
            // 添加可以处理cookie的处理程序
            .AddCookie("Cookies")
            // 用于配置执行OpenID Connect协议的处理程序
            .AddOpenIdConnect("oidc", options =>
            {
                // 1、生成id_token
                options.Authority = "https://localhost:6001";    // 受信任令牌服务地址
                options.RequireHttpsMetadata = false;
                options.ClientId = "simple_code";
                options.ClientSecret = "simple_code_secret";
                options.ResponseType = "code";
                options.SaveTokens = true;  // 用于将来自IdentityServer的令牌保留在cookie中

                // 1、添加授权访问api的支持(access_token)
                options.Scope.Add("TeamService");
                options.Scope.Add("offline_access");
            });

            // 添加HttpClient
            services.AddHttpClient();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // 身份认证
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    // RequireAuthorization 自动跳转
                    pattern: "{controller=Home}/{action=Index}/{id?}").RequireAuthorization();
            });
        }
    }
}
