using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace E_Commerce.Web.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection Services)
        {
           Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen(options =>
            {

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    Description = "Enter 'Bearer' followed by space and your token"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new string[] {}
                    }


                });
            });

            return Services;
        }

        public static IServiceCollection AddWebApplicationServices(this IServiceCollection Services)
        {
            Services.Configure<ApiBehaviorOptions>((options) =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationErrors;
            });

            return Services;
        }

        public static IServiceCollection AddJwtServices(this IServiceCollection Services , IConfiguration configuration)
        {

            Services.AddAuthentication((config) =>
            {

                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer((options)=>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience = configuration["JwtOptions:audience"],

                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtOptions:issuer"],

                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:SecretKey"]!))
                };
            });




            return Services;
        }
    }
}
