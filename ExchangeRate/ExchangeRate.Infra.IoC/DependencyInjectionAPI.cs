using DotNetEnv;
using ExchangeRate.Application.API.AwesomeAPI;
using ExchangeRate.Application.Interface.Authenticate;
using ExchangeRate.Application.Interface.AwesomeAPI;
using ExchangeRate.Application.Interface.Currency;
using ExchangeRate.Application.Interface.User;
using ExchangeRate.Application.Service.Authenticate;
using ExchangeRate.Application.Service.Currency;
using ExchangeRate.Application.Service.User;
using ExchangeRate.Application.Settings;
using ExchangeRate.Domain.Interface.Currency;
using ExchangeRate.Domain.Interface.User;
using ExchangeRate.Infra.Data.Context;
using ExchangeRate.Infra.Data.Repository.Currency;
using ExchangeRate.Infra.Data.Repository.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Infra.IoC
{
    public static class DependencyInjectionAPI
    {
        public static IServiceCollection AddInfrastructureAPI(this IServiceCollection services,
       IConfiguration configuration)
        {

            Env.Load();

         
            services.AddHttpClient();
            services.AddScoped<DbContext>();
            services.AddScoped<IAwesomeAPIService, AwesomeAPIService>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IApplicationSettings, ApplicationSettings>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();

            var appSettings = new ApplicationSettings
            {
                URLAwesomeAPI = Environment.GetEnvironmentVariable("URL_AWESOME_API")!,
                ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")!,
                JwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")!,
                JwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")!,
                JwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")!

            };

            services.AddSingleton<IApplicationSettings>(appSettings);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }
            ).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = appSettings.JwtIssuer,
                    ValidAudience = appSettings.JwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(appSettings.JwtSecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });


            return services;
        }
    }
}
