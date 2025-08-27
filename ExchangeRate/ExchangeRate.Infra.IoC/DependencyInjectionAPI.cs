using DotNetEnv;
using ExchangeRate.Application.API.AwesomeAPI;
using ExchangeRate.Application.Interface.AwesomeAPI;
using ExchangeRate.Application.Interface.Currency;
using ExchangeRate.Application.Service.Currency;
using ExchangeRate.Application.Settings;
using ExchangeRate.Domain.Interface.Currency;
using ExchangeRate.Infra.Data.Context;
using ExchangeRate.Infra.Data.Repository.Currency;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            var appSettings = new ApplicationSettings
            {
                URLAwesomeAPI = Environment.GetEnvironmentVariable("URL_AWESOME_API")!,
                ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")!

            };

            services.AddSingleton<IApplicationSettings>(appSettings);

            return services;
        }
    }
}
