using ExchangeRate.Application.API.AwesomeAPI;
using ExchangeRate.Application.Interface.AwesomeAPI;
using ExchangeRate.Application.Interface.Currency;
using ExchangeRate.Application.Service;
using ExchangeRate.Domain.Interface;
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

            services.AddScoped<DbContext>();
            services.AddScoped<IAwesomeAPIService, AwesomeAPIService>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<ICurrencyService, CurrencyService>();

            return services;
        }
    }
}
