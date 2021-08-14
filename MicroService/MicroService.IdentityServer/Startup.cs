using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.IdentityServer
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
            #region Identity Server 4
            // 这里是开发和测试中使用
            services.AddIdentityServer()
                // 生成秘钥
                .AddDeveloperSigningCredential()
                // 将API范围配置添加到内存中
                .AddInMemoryApiScopes(Config.ApiScopes)
                // 将客户端添加到内存中
                .AddInMemoryClients(Config.Clients)
                // 添加用户
                .AddTestUsers(Config.Users)
                // 5、openid身份
                .AddInMemoryIdentityResources(Config.Ids);
            #endregion

            services.AddControllersWithViews();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MicroService.IdentityServer", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroService.IdentityServer v1"));
            }

            #region 添加Identity Server 4中间件
            app.UseIdentityServer();
            #endregion

            // 静态资源中间件
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();

            // 认证中间件
            app.UseAuthentication();
            // 授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
