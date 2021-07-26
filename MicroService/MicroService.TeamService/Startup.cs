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
            // 1、注册上下文到IOC容器
            services.AddDbContext<TeamDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            // 2、注册团队service
            services.AddScoped<ITeamService, TeamServiceImpl>();

            // 3、注册团队仓储
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

            // 获取命令行里面的Ip
            string ip = Configuration["ip"];
            // 获取命令行里面的端口号
            int port = int.Parse(Configuration["port"]);

            // 1、创建consul客户端连接
            var consulClient = new ConsulClient(configuration =>
            {
                //1.1 建立客户端和服务端连接
                configuration.Address = new Uri("http://127.0.0.1:8500");
            });
            // 2、获取服务内部地址

            // 3、创建consul服务注册对象
            var registration = new AgentServiceRegistration()
            {
                // 编号，ID是唯一的，集群的时候根据编号去找到这个唯一的服务
                ID = Guid.NewGuid().ToString(),
                // 服务名字，做集群的时候根据服务名字获取所有的服务地址集合
                Name = "teamservice",
                // 服务地址
                Address = ip,
                // 服务端口
                Port = port,
                Tags = null,
                // 心跳检测
                Check = new AgentServiceCheck
                {
                    // 3.1、consul健康检查超时时间
                    Timeout = TimeSpan.FromSeconds(10),
                    // 3.2、服务停止5秒后注销服务
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    // 3.3、consul健康检查地址
                    HTTP = $"{ip}:{port}/api/HealthCheck",
                    // 3.4 consul健康检查间隔时间，即隔多长时间进行一次健康检查
                    Interval = TimeSpan.FromSeconds(10),
                }
            };

            // 4、注册服务
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
