using JB.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JB.Email
{
    public static class EmailServiceExtension
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services, string? apiKey = null)
        {
            return services.AddScoped<IWrapper>((provider) => { return Factory.CreateEmailWrapper(apiKey); });
        }
    }
}
