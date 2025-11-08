using Microsoft.Extensions.DependencyInjection;
using Service.Profiles;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddAutoMapper(config => config.AddProfile(new ProductProfile()), typeof(Service.AssemblyReference).Assembly);
            Services.AddScoped<IServiceManager, ServiceManager>();
            Services.AddScoped<ICacheService, CacheService>();
            return Services;
        }
    }
}
