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
            // 1������������֤
            // ����ʹ��cookie�����ص�¼�û���ͨ����Cookies����ΪDefaultScheme�������ҽ�DefaultChallengeScheme����Ϊoidc��
            // ��Ϊ��������Ҫ�û���¼ʱ�����ǽ�ʹ��OpenID ConnectЭ�顣
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc"; // openid connect
            })
            // ���ӿ��Դ���cookie�Ĵ�������
            .AddCookie("Cookies")
            // ��������ִ��OpenID ConnectЭ��Ĵ�������
            .AddOpenIdConnect("oidc", options =>
            {
                // 1������id_token
                options.Authority = "https://localhost:6001";    // ���������Ʒ����ַ
                options.RequireHttpsMetadata = false;
                options.ClientId = "simple_code";
                options.ClientSecret = "simple_code_secret";
                options.ResponseType = "code";
                options.SaveTokens = true;  // ���ڽ�����IdentityServer�����Ʊ�����cookie��

                // 1��������Ȩ����api��֧��(access_token)
                options.Scope.Add("TeamService");
                options.Scope.Add("offline_access");
            });

            // ����HttpClient
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

            // ������֤
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    // RequireAuthorization �Զ���ת
                    pattern: "{controller=Home}/{action=Index}/{id?}").RequireAuthorization();
            });
        }
    }
}