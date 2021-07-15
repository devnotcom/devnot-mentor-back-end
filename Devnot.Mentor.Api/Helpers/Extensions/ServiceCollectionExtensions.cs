using System;
using DevnotMentor.Api.Configuration.Context;
using DevnotMentor.Api.Configuration.Environment;
using DevnotMentor.Api.Repositories;
using DevnotMentor.Api.Repositories.Interfaces;
using DevnotMentor.Api.Services;
using DevnotMentor.Api.Services.Interfaces;
using DevnotMentor.Api.Utilities.Email;
using DevnotMentor.Api.Utilities.Email.SmtpMail;
using DevnotMentor.Api.Utilities.File;
using DevnotMentor.Api.Utilities.File.Local;
using DevnotMentor.Api.Utilities.Security.Token;
using DevnotMentor.Api.Utilities.Security.Token.Jwt;
using Microsoft.Extensions.DependencyInjection;

namespace DevnotMentor.Api.Helpers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            
            services.AddSingleton<TokenAuthentication>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMentorService, MentorService>();
            services.AddScoped<IMenteeService, MenteeService>();

            services.AddScoped<IMailService, SmtpMailService>();
            services.AddScoped<IFileService, LocalFileService>();

            services.AddSingleton<ITokenService, JwtTokenService>();
            //services.AddSingleton<IHashService, Sha256HashService>();

            services.AddSingleton<IDevnotConfigurationContext, DevnotConfigurationContext>();
            services.AddSingleton<IEnvironmentService, EnvironmentService>();

            services.AddCustomSwagger();

            return services;
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ILoggerRepository, LoggerRepository>();
            services.AddScoped<IMenteeLinksRepository, MenteeLinksRepository>();
            services.AddScoped<IMenteeRepository, MenteeRepository>();
            services.AddScoped<IMenteeTagsRepository, MenteeTagsRepository>();
            services.AddScoped<IMentorApplicationsRepository, MentorApplicationsRepository>();
            services.AddScoped<IMentorLinksRepository, MentorLinksRepository>();
            services.AddScoped<IMentorMenteePairsRepository, MentorMenteePairsRepository>();
            services.AddScoped<IMentorRepository, MentorRepository>();
            services.AddScoped<IMentorTagsRepository, MentorTagsRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}