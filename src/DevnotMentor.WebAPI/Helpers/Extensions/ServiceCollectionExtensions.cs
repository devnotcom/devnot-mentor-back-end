using DevnotMentor.Business.Services;
using DevnotMentor.Business.Services.Interfaces;
using DevnotMentor.Business.Utilities.Email;
using DevnotMentor.Business.Utilities.Email.SmtpMail;
using DevnotMentor.Business.Utilities.File;
using DevnotMentor.Business.Utilities.File.Local;
using DevnotMentor.Business.Utilities.Security.Hash;
using DevnotMentor.Business.Utilities.Security.Hash.Sha256;
using DevnotMentor.Business.Utilities.Security.Token;
using DevnotMentor.Business.Utilities.Security.Token.Jwt;
using DevnotMentor.Configuration.Context;
using DevnotMentor.Configuration.Environment;
using DevnotMentor.Data;
using DevnotMentor.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DevnotMentor.WebAPI.Helpers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataRepositories(this IServiceCollection services)
        {
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IMenteeLinkRepository, MenteeLinkRepository>();
            services.AddScoped<IMenteeRepository, MenteeRepository>();
            services.AddScoped<IMenteeTagRepository, MenteeTagRepository>();
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IMentorLinkRepository, MentorLinkRepository>();
            services.AddScoped<IMentorshipRepository, MentorshipRepository>();
            services.AddScoped<IMentorRepository, MentorRepository>();
            services.AddScoped<IMentorTagRepository, MentorTagRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection AddBussinessServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMentorService, MentorService>();
            services.AddScoped<IMenteeService, MenteeService>();
            services.AddScoped<IMentorshipService, MentorshipService>();
            services.AddScoped<IApplicationService, ApplicationService>();
            return services;
        }

        public static IServiceCollection AddBusinessUtilities(this IServiceCollection services)
        {
            services.AddScoped<IMailService, SmtpMailService>();
            services.AddScoped<IFileService, LocalFileService>();

            services.AddSingleton<ITokenService, JwtTokenService>();
            services.AddSingleton<IHashService, Sha256HashService>();
            services.AddSingleton<IDevnotConfigurationContext, DevnotConfigurationContext>();
            services.AddSingleton<IEnvironmentService, EnvironmentService>();

            return services;
        }
    }
}