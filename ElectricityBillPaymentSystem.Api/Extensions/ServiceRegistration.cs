using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ElectricityBillPaymentSystem.Core.Abstractions;
using ElectricityBillPaymentSystem.Infrastructure;
using ElectricityBillPaymentSystem.Infrastructure.Contexts;
using ElectricityBillPaymentSystem.Infrastructure.Repositories;
using ElectricityBillPaymentSystem.Services;
using ElectricityBillPaymentSystem.Domain.Entities;
using SalesOrderAPI.Core.Services;

namespace ElectricityBillPaymentSystem.Api.Extensions;

public static class ServiceRegistration
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Fast api",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Scheme = "Bearer"
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
                    },
                    new string[] { }
                }
            });
        });

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                optionsBuilder =>
                {
                    optionsBuilder.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name);
                }));


        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var key = Encoding.UTF8.GetBytes(configuration.GetSection("JWT:Key").Value);
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false
            };
        });

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<IBillService, BillService>();
        services.AddScoped<IAuthService, AuthService>();

        //services.AddScoped(typeof(IRepository<Bill>), typeof(Repository<Bill>));
        //services.AddScoped(typeof(IRepository<Wallet>), typeof(Repository<Wallet>));
        //services.AddScoped(typeof(IRepository<User>), typeof(Repository<User>));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

    }
}