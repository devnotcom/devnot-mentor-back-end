using System;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Services;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.Utilities.Security.Token;
using DevnotMentor.Api.Configuration.Context;
using DevnotMentor.Api.Configuration.Environment;
using DevnotMentor.Api.Middlewares;
using DevnotMentor.Api.Utilities.Security.Hash;
using DevnotMentor.Api.Utilities.Security.Hash.Sha256;
using DevnotMentor.Api.Utilities.Email;
using DevnotMentor.Api.Repositories;
using DevnotMentor.Api.Repositories.Interfaces;
using DevnotMentor.Api.Utilities.Email.SmtpMail;
using DevnotMentor.Api.Utilities.File;
using DevnotMentor.Api.Utilities.File.Local;
using DevnotMentor.Api.Utilities.Security.Token.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

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
            var connectionString = EnvironmentService.StaticConfiguration["ConnectionStrings:SQLServerConnectionString"];
            services.AddDbContext<MentorDBContext>(options => options.UseSqlServer(connectionString));
            services.AddSingleton<TokenAuthentication>();

            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie()
            .AddGitHub(options =>
            {
                options.ClientId = EnvironmentService.StaticConfiguration["GitHub:Client:ID"];
                options.ClientSecret = EnvironmentService.StaticConfiguration["GitHub:Client:Secret"];
                options.CallbackPath = new PathString("/auth-github/");
                options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                options.TokenEndpoint = "https://github.com/login/oauth/access_token";
                options.UserInformationEndpoint = "https://api.github.com/user";

                options.ClaimActions.MapJsonKey("github:id", "id");
                options.ClaimActions.MapJsonKey("github:name", "name");
                options.ClaimActions.MapJsonKey("github:login", "login");
                options.ClaimActions.MapJsonKey("github:avatar", "avatar_url");

                options.SaveTokens = true;

                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async ctx =>
                    {
                        var request = new HttpRequestMessage(HttpMethod.Get, ctx.Options.UserInformationEndpoint);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ctx.AccessToken);
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var response = await ctx.Backchannel.SendAsync(request, ctx.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        var jsonDocumentFromResponse = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                        ctx.RunClaimActions(jsonDocumentFromResponse.RootElement);
                    }
                };
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMentorService, MentorService>();
            services.AddScoped<IMenteeService, MenteeService>();

            services.AddScoped<IMailService, SmtpMailService>();
            services.AddScoped<IFileService, LocalFileService>();

            services.AddSingleton<ITokenService, JwtTokenService>();
            services.AddSingleton<IHashService, Sha256HashService>();

            services.AddSingleton<IDevnotConfigurationContext, DevnotConfigurationContext>();
            services.AddSingleton<IEnvironmentService, EnvironmentService>();

            #region Repositories

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
