using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using UserManagment.API.BL;
using UserManagment.API.DbData;
using UserManagment.API.Models;

namespace UserManagment.API.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http, // 👈 use Http not ApiKey
                    Scheme = "bearer",              // 👈 this lower-case "bearer" makes Swagger auto-append Bearer
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token only (without 'Bearer ' prefix)."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "bearer",
                Name = "Authorization",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
            });

            services.Configure<JwtModel>(configuration.GetSection("Jwt"));
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataContext>(option =>
            {
                option.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserServices, UserServices>();


            return services;
        }
    }
}
