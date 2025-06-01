using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.Extensions.DependencyInjection;
using Service.Contract;
using Services;

namespace Repository
{
    public static class ServiceExtensions
    {
        public static void ConfigureServiceManager(this IServiceCollection services) =>
 services.AddScoped<IServiceManager, ServiceManager>();
    }
}
