using LayeredStorageApi.BackgroundServices;
using LayeredStorageApi.BL;
using LayeredStorageApi.BL.Storage;
using LayeredStorageApi.DbData;
using LayeredStorageApi.Services.Implementations;
using LayeredStorageApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Project.Common.Interfaces.Data;
using Project.Common.Interfaces.Services;
using Project.Common.Models;
using Project.Common.Models.Core;
using Project.Common.Services.Redis;

namespace LayeredStorageApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
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

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataLayerContext>(option =>
            {
                option.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });




            services.UseRedisCache(configuration);
            services.Configure<CacheConfig>(configuration.GetSection(nameof(CacheConfig)));
            services.AddScoped<IIncertBulk, IncertBulk>();
            services.AddSingleton<ICache, RedisCache>();
            services.AddHostedService<FileCleanupService>();
            services.AddScoped<IDataRepository, DataRepository>();
            services.AddScoped<RedisStorage>();
            services.AddScoped<FileStorage>();
            services.AddScoped<DbStorage>();
            services.AddScoped<IStorageFactory, StorageFactory>();
            services.AddScoped<IFileService, FileService>();
            services.AddHttpClient<IFileStorage, SeaweedStorage>(client =>
            {
                client.BaseAddress = new Uri(configuration["FileService:SeaweedUrl"]);
            });



            return services;
        }
    }
}
