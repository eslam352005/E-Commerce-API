using Domain.Contracts;
using System.Threading.Tasks;

namespace E_Commerce.Web.Extensions
{
    public static class WebApplicationRegistration
    {
        public static async Task SeedDataAsync(this WebApplication app)
        {
            var scope = app.Services.CreateScope();

            var seed = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            await seed.DataSeedAsync();
            await seed.IdentityDataSeedAsync();
        }
    }
}
