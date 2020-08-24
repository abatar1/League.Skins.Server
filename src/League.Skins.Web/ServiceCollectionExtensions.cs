using System;
using League.Skins.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Entities;
using Newtonsoft.Json;

namespace League.Skins.Web
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterConfig<T>(this IServiceCollection services, IConfiguration configuration,
            IHostEnvironment hostEnvironment, ILogger logger)
            where T : class
        {
            const string name = nameof(T);
            services.Configure<T>(model =>
            {
                if (hostEnvironment.IsDevelopment())
                {
                    configuration.Bind(name, model);
                }
                else
                {
                    var value = Environment.GetEnvironmentVariable(name);
                    if (value == null)
                    {
                        var message = $"Environment value {name} is null.";
                        logger.LogCritical(message);
                        throw new ArgumentException(message);
                    }

                    try
                    {
                        JsonConvert.PopulateObject(value, model);
                    }
                    catch (Exception)
                    {
                        logger.LogCritical($"Environment value {name} given in wrong format. Value:\r\n{value}");
                        throw;
                    }
                }
            });
        }

        private static void RegisterMongodbSettings(this IServiceCollection services, IConfiguration configuration,
            IHostEnvironment hostEnvironment, ILogger logger) =>
            services.RegisterConfig<SkinsStatisticsDatabaseSettings>(configuration, hostEnvironment, logger);

        public static void RegisterMongodbServices(this IServiceCollection services, IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            var configurationLogger =
                LoggerFactory.Create(builder => { builder.AddConsole(); }).CreateLogger<Startup>();

            services.Configure<SkinsStatisticsDatabaseSettings>(
                configuration.GetSection(nameof(SkinsStatisticsDatabaseSettings)));
            services.RegisterMongodbSettings(configuration, hostEnvironment, configurationLogger);
            services.AddSingleton<ISkinsStatisticsDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<SkinsStatisticsDatabaseSettings>>().Value);
        }
    }
}
