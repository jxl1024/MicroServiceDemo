using IdentityServer4.AccessTokenValidation;
using MicroService.Gateway.IdentityServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.Gateway
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
    //// 1������IdentityServer
    //var authenticationProviderKey = "OcelotKey";
    //services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
    //        .AddIdentityServerAuthentication(authenticationProviderKey,options => {
    //            options.Authority = "http://localhost:6000"; // 1����Ȩ���ĵ�ַ
    //            options.ApiName = "OcelotService"; // 2��api����(��Ŀ��������)
    //            options.RequireHttpsMetadata = false; // 3��httpsԪ���ݣ�����Ҫ
    //            options.SupportedTokens = SupportedTokens.Both;
    //        });

    //  ����IdentityServer
    var identityServerOptions = new IdentityServerOptions();
    Configuration.Bind("IdentityServerOptions", identityServerOptions);
    services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            .AddIdentityServerAuthentication(identityServerOptions.IdentityScheme, options =>
            {
                options.Authority = identityServerOptions.AuthorityAddress; // 1����Ȩ���ĵ�ַ
                options.ApiName = identityServerOptions.ResourceName; // 2��api����(��Ŀ��������)
                options.RequireHttpsMetadata = false; // 3��httpsԪ���ݣ�����Ҫ
            });

    // 1���������Ocelot��ioc����
    services.AddOcelot().AddConsul();
    services.AddControllers();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "MicroService.Gateway", Version = "v1" });
    });
}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroService.Gateway v1"));
            }

            // 2��ʹ������
            app.UseOcelot().Wait();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
