using JB.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JB.Weather
{
    public static class WeatherServiceExtension
    {
        public static IServiceCollection AddWeatherService(this IServiceCollection services)
        {
            return services.AddScoped<IWrapper>((provider) => { return Factory.CreateWeatherWrapper(); });
        }
    }
}
