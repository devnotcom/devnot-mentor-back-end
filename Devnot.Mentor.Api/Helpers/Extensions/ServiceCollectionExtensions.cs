using System;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.Configuration.Environment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.Cookies;
using DevnotMentor.Api.CustomEntities.Auth;
using DevnotMentor.Api.Services;
using DevnotMentor.Api.Utilities.Email;
using DevnotMentor.Api.Utilities.Security.Token;
using DevnotMentor.Api.Configuration.Context;
using DevnotMentor.Api.Repositories.Interfaces;
using DevnotMentor.Api.Repositories;
using DevnotMentor.Api.Utilities.Email.SmtpMail;
using DevnotMentor.Api.Utilities.Security.Token.Jwt;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace DevnotMentor.Api.Helpers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {

            services.AddSingleton<TokenAuthentication>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMentorService, MentorService>();
            services.AddScoped<IMenteeService, MenteeService>();

            services.AddScoped<IMailService, SmtpMailService>();
            //services.AddScoped<IFileService, LocalFileService>();

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

        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie()
            .AddGitHub(options =>
            {
                options.ClientId = EnvironmentService.StaticConfiguration["GitHub:Client:ID"];
                options.ClientSecret = EnvironmentService.StaticConfiguration["GitHub:Client:Secret"];
                options.CallbackPath = new PathString("/auth/github/");

                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async ctx => { var oAuthUser = await GetOAuthUser(OAuthType.GitHub, ctx); await SignIn(oAuthUser, ctx.HttpContext); }
                };
            })
            .AddGoogle(options =>
            {
                options.ClientId = EnvironmentService.StaticConfiguration["Google:Client:ID"];
                options.ClientSecret = EnvironmentService.StaticConfiguration["Google:Client:Secret"];
                options.CallbackPath = new PathString("/auth/google/");

                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async ctx => { var oAuthUser = await GetOAuthUser(OAuthType.Google, ctx); await SignIn(oAuthUser, ctx.HttpContext); }
                };
            });

            return services;
        }

        private static async Task<OAuthUser> GetOAuthUser(OAuthType oAuthType, OAuthCreatingTicketContext ctx)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, ctx.Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ctx.AccessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await ctx.Backchannel.SendAsync(request, ctx.HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            if (oAuthType == OAuthType.Google)
            {
                var googleResponse = JsonConvert.DeserializeObject<GoogleResponse>(responseContent);
                return new OAuthUser
                {
                    Id = googleResponse.id,
                    IdentifierProperty = googleResponse.email,
                    FullName = googleResponse.name,
                    ProfilePictureUrl = googleResponse.picture,
                };
            }

            var gitHubResponse = JsonConvert.DeserializeObject<GitHubResponse>(responseContent);
            return new OAuthUser
            {
                Id = gitHubResponse.id,
                IdentifierProperty = gitHubResponse.login,
                FullName = gitHubResponse.name,
                ProfilePictureUrl = gitHubResponse.avatar_url,
            };
        }

        private static async Task SignIn(OAuthUser oAuthUser, HttpContext httpContext)
        {
            var userService = httpContext.RequestServices.GetService<IUserService>();
            var signInResponse = await userService.SignInAsync(oAuthUser);
            if (signInResponse.Success)
            {
                httpContext.Response.Headers.Add("auth-token", signInResponse.Data.Token);
                httpContext.Response.Headers.Add("auth-token-expiry-date", signInResponse.Data.TokenExpiryDate.ToString());
            }
            else
            {
                httpContext.Response.StatusCode = 500;
            }
        }
    }
}