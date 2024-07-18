using JB.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JB.Calendar
{
    public static class CalendarServiceExtension
    {
        public static IServiceCollection AddCalendarService(this IServiceCollection services)
        {
            return services.AddScoped<IWrapper, GoogleCalendar.Wrapper>();
        }
    }
}
