using JB.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JB.Blob
{
    public static class BlobServiceExtension
    {
        public static IServiceCollection AddBlobService(this IServiceCollection services, string? connectionString = null)
        {
            return services.AddScoped<IWrapper>((provider) => { return Factory.CreateBlobWrapper(connectionString); });
        }
    }
}
