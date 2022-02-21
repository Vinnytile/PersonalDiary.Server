using BusinessLogic.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedData.Models;
using System.Text;

namespace PersonalDiary.Server.ServiceExtensions
{
    public static class JwtAuthenticationExtension
    {
        public static IServiceCollection AddJwtAuthenticationService(this IServiceCollection services, JwtSettings jwtSettings)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(c =>
            {
                c.SaveToken = true;
                c.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });

            services.AddSingleton<IJwtService, JwtService>();
            return services;
        }
    }
}
