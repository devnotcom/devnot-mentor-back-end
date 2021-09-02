using System;
using DevnotMentor.Data.Entities;
using DevnotMentor.Services;
using DevnotMentor.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Utilities.Security.Token;
using DevnotMentor.Configurations.Context;
using DevnotMentor.Configurations.Environment;
using DevnotMentor.Api.Middlewares;
using DevnotMentor.Utilities.Security.Hash;
using DevnotMentor.Utilities.Security.Hash.Sha256;
using DevnotMentor.Utilities.Email;
using DevnotMentor.Data;
using DevnotMentor.Data.Interfaces;
using DevnotMentor.Utilities.Email.SmtpMail;
using DevnotMentor.Utilities.File;
using DevnotMentor.Utilities.File.Local;
using DevnotMentor.Utilities.Security.Token.Jwt;

namespace DevnotMentor.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            System.Console.WriteLine("TEST");

            var connectionString = EnvironmentService.StaticConfiguration["ConnectionStrings:SQLServerConnectionString"];
            System.Console.WriteLine(connectionString);
            services.AddDbContext<MentorDBContext>(options => options.UseSqlServer(connectionString));

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton<TokenAuthentication>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMentorService, MentorService>();
            services.AddScoped<IMenteeService, MenteeService>();
            services.AddScoped<IMentorshipService, MentorshipService>();
            services.AddScoped<IApplicationService, ApplicationService>();


            services.AddScoped<IMailService, SmtpMailService>();
            services.AddScoped<IFileService, LocalFileService>();

            services.AddSingleton<ITokenService, JwtTokenService>();
            services.AddSingleton<IHashService, Sha256HashService>();
            services.AddSingleton<IDevnotConfigurationContext, DevnotConfigurationContext>();
            services.AddSingleton<IEnvironmentService, EnvironmentService>();

            #region Repositories

            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IMenteeLinksRepository, MenteeLinksRepository>();
            services.AddScoped<IMenteeRepository, MenteeRepository>();
            services.AddScoped<IMenteeTagsRepository, MenteeTagsRepository>();
            services.AddScoped<IApplicationsRepository, IApplicationRepository>();
            services.AddScoped<IMentorLinksRepository, MentorLinksRepository>();
            services.AddScoped<IMentorshipsRepository, MentorshipsRepository>();
            services.AddScoped<IMentorRepository, MentorRepository>();
            services.AddScoped<IMentorTagsRepository, MentorTagsRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            #endregion

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

            services.AddCustomSwagger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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