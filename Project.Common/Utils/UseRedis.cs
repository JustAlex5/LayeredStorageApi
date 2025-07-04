using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Common.Models;
using Project.Common.Services;
using StackExchange.Redis;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

public static class UseRedis
{
    public static void UseRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        var cacheConfig = configuration.GetSection("CacheConfig").Get<CacheConfig>();
        var connection = cacheConfig.ConnectionString.Split(",");
        var configurationOptions = new ConfigurationOptions
        {
            AbortOnConnectFail = false,
        };

        foreach (var entry in connection)
        {
            if (entry.StartsWith("password", StringComparison.OrdinalIgnoreCase))
                configurationOptions.Password = entry.Split('=')[1];
            else if (entry.StartsWith("ServiceName", StringComparison.OrdinalIgnoreCase))
                configurationOptions.ServiceName = entry.Split('=')[1];
            else
                configurationOptions.EndPoints.Add(entry);
        }

        if (cacheConfig.UseSsl)
            AddSsl(configurationOptions, cacheConfig);

        services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(configurationOptions));
        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = cacheConfig.ConnectionString;
            opt.InstanceName = cacheConfig.InstanceName;
        });
        services.AddSingleton<ICache, RedisCache>();


    }

    private static void AddSsl(ConfigurationOptions options, CacheConfig cacheConfig)
    {
        options.Ssl = true;
        options.SslHost = cacheConfig.HostName;
        options.CheckCertificateRevocation = false;
        options.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;

        options.CertificateSelection += delegate
        {
            try
            {
                Console.WriteLine($"Going to {cacheConfig.PfxPath}");
                if (File.Exists(cacheConfig.PfxPath))
                {
                    return new X509Certificate2(cacheConfig.PfxPath, cacheConfig.PfxPass);
                }
                else
                {
                    Console.WriteLine("Pfx file doesn't exist");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        };

        options.CertificateValidation += ValidateCertificate;
    }

    private static bool ValidateCertificate(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors policyErrors)
    {
        return certificate != null;
    }
}
