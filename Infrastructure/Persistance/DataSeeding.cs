using Domain.Contracts;
using Domain.Models.Identities;
using Domain.Models.Orders;
using Domain.Models.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public  class DataSeeding(StoreDbContext _context,
                              UserManager<ApplicationUser> _userManager,
                              RoleManager<IdentityRole> _roleManager) : IDataSeeding
    {
        public async Task DataSeedAsync()
        {
            try
            {
                if ((await _context.Database.GetPendingMigrationsAsync()).Any())
                {
                    await _context.Database.MigrateAsync();
                }

                if (!_context.productBrands.Any())
                {
                    var brandsData = File.OpenRead(@"..\Infrastructure\Persistance\DataSeed\brands.json");
                    var brands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(brandsData);
                    if (brands != null && brands.Any())
                    {
                        await _context.productBrands.AddRangeAsync(brands);
                    }

                }

                if (!_context.productTypes.Any())
                {
                    var typesData = File.OpenRead(@"..\Infrastructure\Persistance\DataSeed\types.json");
                    var types = await JsonSerializer.DeserializeAsync<List<ProductType>>(typesData);
                    if (types != null && types.Any())
                    {
                        await _context.productTypes.AddRangeAsync(types);
                    }

                }

                if (!_context.products.Any())
                {
                    var productsData = File.OpenRead(@"..\Infrastructure\Persistance\DataSeed\products.json");
                    var Products = await JsonSerializer.DeserializeAsync<List<Product>>(productsData);
                    if (Products != null && Products.Any())
                    {
                        await _context.products.AddRangeAsync(Products);
                    }

                }
                if (!_context.Set<DeliveryMethod>().Any())
                {
                    var deliveryData = File.OpenRead(@"..\Infrastructure\Persistance\DataSeed\delivery.json");
                    var delivers = await JsonSerializer.DeserializeAsync<List<DeliveryMethod>>(deliveryData);
                    if (delivers != null && delivers.Any())
                    {
                        await _context.Set<DeliveryMethod>().AddRangeAsync(delivers);
                    }

                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               
            }

        }

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!_userManager.Users.Any())
                {
                    var user1 = new ApplicationUser()
                    {
                        Email = "Mohamed@gmail.com",
                        DisplayName = "Mohamed Aly",
                        UserName = "MohamedAly",
                        PhoneNumber = "1234567890",
                    };
                    var user2 = new ApplicationUser()
                    {
                        Email = "Salma@gmail.com",
                        DisplayName = "Salma Mohamed",
                        UserName = "SalmaMohamed",
                        PhoneNumber = "01150734483",
                    };

                    await _userManager.CreateAsync(user1, "P@ssW0rd");
                    await _userManager.CreateAsync(user2, "P@ssW0rd");

                    await _userManager.AddToRoleAsync(user1, "Admin");
                    await _userManager.AddToRoleAsync(user2, "SuperAdmin");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
    }

    
}
