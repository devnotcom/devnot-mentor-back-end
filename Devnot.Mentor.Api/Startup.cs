using System;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Services.Interfaces;
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
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DevnotMentor.Api.CustomEntities.Auth;
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

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie() //todo: remove cookie and implement jwt bearer
            .AddGitHub(options =>
            {
                options.ClientId = EnvironmentService.StaticConfiguration["GitHub:Client:ID"];
                options.ClientSecret = EnvironmentService.StaticConfiguration["GitHub:Client:Secret"];
                options.CallbackPath = new PathString("/auth/github/");
                options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                options.TokenEndpoint = "https://github.com/login/oauth/access_token";
                options.UserInformationEndpoint = "https://api.github.com/user";

                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async ctx =>
                    {
                        var request = new HttpRequestMessage(HttpMethod.Get, ctx.Options.UserInformationEndpoint);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ctx.AccessToken);
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var response = await ctx.Backchannel.SendAsync(request, ctx.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        var gitHubResponse = JsonConvert.DeserializeObject<GitHubResponse>(await response.Content.ReadAsStringAsync());
                        await GitHubAuthAsync(gitHubResponse, ctx.HttpContext);
                    }
                };
            });

            services.AddCustomServices();
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

        public static async Task GitHubAuthAsync(GitHubResponse gitHub, HttpContext httpContext)
        { //todo: find another way to return token
            var userService = httpContext.RequestServices.GetService<IUserService>();
            var authResponse = await userService.GitHubAuth(gitHub.id, gitHub.name, gitHub.login, gitHub.avatar_url);
            if (authResponse.Success)
            {
                httpContext.Response.Headers.Add("auth-token", authResponse.Data.Token);
                httpContext.Response.Headers.Add("auth-token-expiry-date", authResponse.Data.TokenExpiryDate.ToString());
            }
            else
            {
                httpContext.Response.StatusCode = 500;
            }
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
