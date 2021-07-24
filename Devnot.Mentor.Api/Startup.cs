using System;
using DevnotMentor.Api.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.Configuration.Environment;
using DevnotMentor.Api.Middlewares;
using Microsoft.AspNetCore.Http;
using DevnotMentor.Api.Helpers.Extensions;

namespace DevnotMentor.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = EnvironmentService.StaticConfiguration["ConnectionStrings:SQLServerConnectionString"];
            services.AddDbContext<MentorDBContext>(options => options.UseSqlServer(connectionString));
            services.AddSingleton<TokenAuthentication>();

            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));

            services.AddCustomServices();
            services.AddCustomAuthentication();
            services.AddRepositories();

            services.AddCors(options =>
            {
                options
                .AddPolicy("AllowMyOrigin", builder =>
                    builder
                    //.WithOrigins("http://localhost:8080")
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateAttribute));
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                MinimumSameSitePolicy = SameSiteMode.Lax
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseCors("AllowMyOrigin");
            app.UseCustomSwagger();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
