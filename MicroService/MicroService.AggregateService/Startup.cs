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
            // 1���Զ����쳣����(�û��洦��)
            var fallbackResponse = new HttpResponseMessage
            {
                Content = new StringContent("ϵͳ����æ�����Ժ�����"),// ���ݣ��Զ�������
                StatusCode = HttpStatusCode.GatewayTimeout // 504
            };

            // ע��IHTTPClientFactory
            //services.AddHttpClient();
            #region polly����
            {
                /*services.AddHttpClient("mrico")
                // 1.1 ����(�����쳣�������Զ��崦��)
                .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<Exception>().FallbackAsync(fallbackResponse, async b =>
                {
                    // 1��������ӡ�쳣
                    Console.WriteLine($"��ʼ����,�쳣��Ϣ��{b.Exception.Message}");
                    // 2�������������
                    //Console.WriteLine($"����������Ӧ��{}");
                    await Task.CompletedTask;
                }))
                // 1.2 �۶ϻ���
                .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<Exception>().CircuitBreakerAsync(3, TimeSpan.FromSeconds(10), (ex, ts) =>
                {
                    Console.WriteLine($"��·���������쳣��Ϣ��{ex.Exception.Message}");
                    Console.WriteLine($"��·������ʱ�䣺{ts.TotalSeconds}s");
                }, () =>
                {
                    Console.WriteLine($"��·������");
                }, () =>
                {
                    Console.WriteLine($"��·���뿪��(һ�Ὺ��һ���)");
                }))
                // 1.3 ʧ������
                .AddPolicyHandler(Policy<HttpResponseMessage>
                  .Handle<Exception>()
                  .RetryAsync(3)
                )
                //1.4����ʱ
                .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(2)));*/
            }
            #endregion

            // 1.2 ��װ֮��ĵ���PollyHttpClient
            services.AddPollyHttpClient("mrico", options =>
            {
                options.TimeoutTime = 1; // 1����ʱʱ��
                options.RetryCount = 3;// 2�����Դ���
                options.CircuitBreakerOpenFallCount = 2;// 3���۶�������(���ٴ�ʧ�ܿ���)
                options.CircuitBreakerDownTime = 100;// 4���۶�������ʱ��
                options.httpResponseMessage = fallbackResponse;// 5����������
            });

            // 2��ע��team����
            services.AddScoped<ITeamServiceClient, HttpTeamServiceClient>();
            services.AddScoped<IMemeberServiceClient, HttpMemberServiceClient>();

            // 3��ע��consul������
            //services.AddConsulDiscovery();
            services.AddHttpClientConsul();

            // 4��ע�Ḻ�ؾ���
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
