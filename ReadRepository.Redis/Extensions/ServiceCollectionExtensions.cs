using System;
using DigiTFactory.Libraries.ReadRepository.Redis.Configuration;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace DigiTFactory.Libraries.ReadRepository.Redis.Extensions
{
    /// <summary>
    /// Расширения для регистрации Redis Read Store в DI контейнере.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрирует Redis Read Store.
        /// </summary>
        /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
        /// <typeparam name="TReadModel">Тип Read-модели.</typeparam>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="configureOptions">Настройки Redis Read Store.</param>
        /// <returns>Коллекция сервисов.</returns>
        public static IServiceCollection AddReadStoreRedis<TBoundedContext, TReadModel>(
            this IServiceCollection services,
            Action<RedisReadStoreOptions> configureOptions)
            where TBoundedContext : IBoundedContext
            where TReadModel : class, IReadModel<TBoundedContext>
        {
            var options = new RedisReadStoreOptions();
            configureOptions(options);

            services.AddSingleton(options);

            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(options.ConnectionString));

            services.AddScoped<IReadRepository<TBoundedContext, TReadModel>,
                RedisReadRepository<TBoundedContext, TReadModel>>();

            services.AddScoped<IReadModelStore<TBoundedContext, TReadModel>,
                RedisReadModelStore<TBoundedContext, TReadModel>>();

            return services;
        }
    }
}
