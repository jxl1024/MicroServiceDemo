using MicroService.AggregateService.Services;
using MicroService.Core.Cluster;
using MicroService.Core.HttpClientConsul;
using MicroService.Core.HttpClientConsul.Extentions;
using MicroService.Core.HttpClientPolly;
using MicroService.Core.Registry.Extentions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;
using System;
using System.Net.Http;

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
            // 1、自定义异常处理(用缓存处理)
            var fallbackResponse = new HttpResponseMessage
            {
                Content = new StringContent("系统正繁忙，请稍后重试"),// 内容，自定义内容
                StatusCode = HttpStatusCode.GatewayTimeout // 504
            };

            // 注册IHTTPClientFactory
            //services.AddHttpClient();
            #region polly配置
            {
                /*services.AddHttpClient("mrico")
                // 1.1 降级(捕获异常，进行自定义处理)
                .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<Exception>().FallbackAsync(fallbackResponse, async b =>
                {
                    // 1、降级打印异常
                    Console.WriteLine($"开始降级,异常消息：{b.Exception.Message}");
                    // 2、降级后的数据
                    //Console.WriteLine($"降级内容响应：{}");
                    await Task.CompletedTask;
                }))
                // 1.2 熔断机制
                .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<Exception>().CircuitBreakerAsync(3, TimeSpan.FromSeconds(10), (ex, ts) =>
                {
                    Console.WriteLine($"断路器开启，异常消息：{ex.Exception.Message}");
                    Console.WriteLine($"断路器开启时间：{ts.TotalSeconds}s");
                }, () =>
                {
                    Console.WriteLine($"断路器重置");
                }, () =>
                {
                    Console.WriteLine($"断路器半开启(一会开，一会关)");
                }))
                // 1.3 失败重试
                .AddPolicyHandler(Policy<HttpResponseMessage>
                  .Handle<Exception>()
                  .RetryAsync(3)
                )
                //1.4、超时
                .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(2)));*/
            }
            #endregion

            // 1.2 封装之后的调用PollyHttpClient
            services.AddPollyHttpClient("mrico", options =>
            {
                options.TimeoutTime = 1; // 1、超时时间
                options.RetryCount = 3;// 2、重试次数
                options.CircuitBreakerOpenFallCount = 2;// 3、熔断器开启(多少次失败开启)
                options.CircuitBreakerDownTime = 100;// 4、熔断器开启时间
                options.httpResponseMessage = fallbackResponse;// 5、降级处理
            });

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
