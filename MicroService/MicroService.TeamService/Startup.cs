using Consul;
using MicroService.TeamService.Context;
using MicroService.TeamService.Repositories;
using MicroService.TeamService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.TeamService
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
            // 1��ע�������ĵ�IOC����
            services.AddDbContext<TeamDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            // 2��ע���Ŷ�service
            services.AddScoped<ITeamService, TeamServiceImpl>();

            // 3��ע���ŶӲִ�
            services.AddScoped<ITeamRepository, TeamRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MicroService.TeamService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroService.TeamService v1"));
            }

            // ��ȡ�����������Ip
            string ip = Configuration["ip"];
            // ��ȡ����������Ķ˿ں�
            int port = int.Parse(Configuration["port"]);

            // 1������consul�ͻ�������
            var consulClient = new ConsulClient(configuration =>
            {
                //1.1 �����ͻ��˺ͷ��������
                configuration.Address = new Uri("http://127.0.0.1:8500");
            });
            // 2����ȡ�����ڲ���ַ

            // 3������consul����ע�����
            var registration = new AgentServiceRegistration()
            {
                // ��ţ�ID��Ψһ�ģ���Ⱥ��ʱ����ݱ��ȥ�ҵ����Ψһ�ķ���
                ID = Guid.NewGuid().ToString(),
                // �������֣�����Ⱥ��ʱ����ݷ������ֻ�ȡ���еķ����ַ����
                Name = "teamservice",
                // �����ַ
                Address = ip,
                // ����˿�
                Port = port,
                Tags = null,
                // �������
                Check = new AgentServiceCheck
                {
                    // 3.1��consul������鳬ʱʱ��
                    Timeout = TimeSpan.FromSeconds(10),
                    // 3.2������ֹͣ5���ע������
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    // 3.3��consul��������ַ
                    HTTP = $"{ip}:{port}/api/HealthCheck",
                    // 3.4 consul���������ʱ�䣬�����೤ʱ�����һ�ν������
                    Interval = TimeSpan.FromSeconds(10),
                }
            };

            // 4��ע�����
            consulClient.Agent.ServiceRegister(registration).Wait();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
