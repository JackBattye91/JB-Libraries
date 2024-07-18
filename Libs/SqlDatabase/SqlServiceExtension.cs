using JB.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JB.SqlDatabase
{
    public static class SqlServiceExtension
    {
        public static IServiceCollection AddSqlService(this IServiceCollection services)
        {
            return services.AddScoped<IWrapper>((provider) => { return Factory.CreateSqlWrapperInstance(); });
        }
    }
}
