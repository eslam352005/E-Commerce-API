using Domain.Contracts;
using E_Commerce.Web.Custom_Middlewares;
using E_Commerce.Web.Extensions;
using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data.Contexts;
using Persistence.Repos;
using Service;
using Service.Profiles;
using ServiceAbstraction;
using Shared.Model_Errors;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           

            #region Services
            builder.Services.AddControllers();

            builder.Services.AddSwaggerService();

            builder.Services.AddInfrastructureService(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddWebApplicationServices();
            builder.Services.AddJwtServices(builder.Configuration);
            #endregion

            var app = builder.Build();

           
            await app.SeedDataAsync();
           

            app.UseMiddleware<CustomMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options=>
                {
                    options.ConfigObject = new ConfigObject()
                    {
                        DisplayRequestDuration = true,
                    };

                    options.DocumentTitle = "E-commerce";

                    options.JsonSerializerOptions = new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    options.DocExpansion(DocExpansion.None);
                    options.EnableFilter();
                    options.EnablePersistAuthorization();

                });
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
