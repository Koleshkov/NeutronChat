using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NeutronChat.Application.Services;
using NeutronChat.Domain.Configurations;
using NeutronChat.Domain.Models;
using NeutronChat.Persistance.Services;
using System.Text;

namespace NeutronChat.Persistance
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AppConnectionString");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit=false;
                opt.Password.RequireNonAlphanumeric=false;
                opt.Password.RequireUppercase=false;
                opt.Password.RequiredLength = 1;
            }).AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();


            AuthenticationConfiguration authenticationConfiguration = new();
            configuration.Bind("Authentication", authenticationConfiguration);

            services.AddSingleton(authenticationConfiguration);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
             {
                 o.TokenValidationParameters = new TokenValidationParameters()
                 {
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.AccessTokenSecretKey)),
                     ValidIssuer = authenticationConfiguration.Issuer,
                     ValidAudience = authenticationConfiguration.Audience,
                     ValidateIssuerSigningKey = true,
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ClockSkew = TimeSpan.Zero
                 };
             });

            services.AddScoped<IAppDbContext, AppDbContext>();

            services.AddSingleton<IAccessTokenGenerator, AccessTokenGenerator>();

            services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();

            services.AddSingleton<IRefreshTokenValidator, RefreshTokenValidator>();

            services.AddSingleton<TokenGenerator>();

            return services;
        }

    }
}
