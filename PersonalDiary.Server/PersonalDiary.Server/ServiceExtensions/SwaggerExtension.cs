using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace PersonalDiary.Server.ServiceExtensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "PersonalDiary API",
                    Description = "An ASP.NET Core Web API for managing PersonalDiary items",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Example Contact",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license")
                    }
                });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[0] }
                };

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { 
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference 
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" 
                            }
                        }, new List<string>() 
                    }
                });
            });

            return services;
        }
    }
}
