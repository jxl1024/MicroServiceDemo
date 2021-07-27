using MicroService.AggregateService.Services;
using MicroService.Core.Cluster;
using MicroService.Core.HttpClientConsul;
using MicroService.Core.HttpClientConsul.Extentions;
using MicroService.Core.Registry.Extentions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace MicroService.AggregateService
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
            // 注册IHTTPClientFactory
            services.AddHttpClient();
            // 2、注册team服务
            services.AddScoped<ITeamServiceClient, HttpTeamServiceClient>();
            services.AddScoped<IMemeberServiceClient, HttpMemberServiceClient>();

            // 3、注册consul服务发现
            //services.AddConsulDiscovery();
            services.AddHttpClientConsul();

            // 4、注册负载均衡
            services.AddSingleton<ILoadBalance, RandomLoadBalance>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MicroService.AggregateService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroService.AggregateService v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
