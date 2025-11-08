using Domain.Contracts;
using Domain.Models.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data.Contexts;
using Persistence.Identity;
using Persistence.Repos;
using Service;
using ServiceAbstraction;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection Services , IConfiguration Configuration)
        {
          Services.AddDbContext<StoreDbContext>(options =>
            {

                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            Services.AddDbContext<StoreIdentityDbContext>(options =>
            {

                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
            });

            Services.AddScoped<IDataSeeding, DataSeeding>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<ICartRepo, CartRepo>();
            Services.AddScoped<ICacheRepo, CacheRepo>();
            Services.AddSingleton<IConnectionMultiplexer>((_) =>
            {
                return ConnectionMultiplexer.Connect(Configuration.GetConnectionString("RedisConnection"));
            });
            Services.AddIdentityCore<ApplicationUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<StoreIdentityDbContext>();
            return Services;
        }
    }
}
