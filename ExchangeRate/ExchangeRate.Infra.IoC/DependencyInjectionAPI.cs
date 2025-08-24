using ExchangeRate.Application.API.AwesomeAPI;
using ExchangeRate.Application.Interface.AwesomeAPI;
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
            services.AddScoped<IAwesomeAPIService, AwesomeAPIService>();


            return services;
        }
    }
}
