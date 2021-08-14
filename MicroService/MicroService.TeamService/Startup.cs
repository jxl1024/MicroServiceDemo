using Consul;
using IdentityServer4.AccessTokenValidation;
using MicroService.Core.Registry.Extentions;
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
using Microsoft.IdentityModel.Tokens;
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

            // 4、添加consul注册中心，加载配置
            services.AddConsulRegistry(Configuration);

            // 5、校验AccessToken,从身份校验中心进行校验
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = Configuration.GetSection("AuthorizationCenter").Value; // 1、授权中心地址
                options.ApiName = "TeamService"; // 2、api名称(项目具体名称)
                options.RequireHttpsMetadata = false; // 3、https元数据，不需要
                options.LegacyAudienceValidation = true;
            });
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

            // 1、consul服务注册
            app.UseConsulRegistry();

            app.UseRouting();

            // 2、认证中间件 先认证后授权
            app.UseAuthentication();
            // 3、授权中间件
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
