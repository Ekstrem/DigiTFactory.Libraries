using System;
using System.Linq;
using Cassandra;
using DigiTFactory.Libraries.ReadRepository.Scylla.Configuration;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DigiTFactory.Libraries.ReadRepository.Scylla.Extensions
{
    /// <summary>
    /// Расширения для регистрации ScyllaDB Read Store в DI контейнере.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрирует ScyllaDB Read Store.
        /// </summary>
        /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
        /// <typeparam name="TReadModel">Тип Read-модели.</typeparam>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="configureOptions">Настройки ScyllaDB Read Store.</param>
        /// <returns>Коллекция сервисов.</returns>
        public static IServiceCollection AddReadStoreScylla<TBoundedContext, TReadModel>(
            this IServiceCollection services,
            Action<ScyllaReadStoreOptions> configureOptions)
            where TBoundedContext : IBoundedContext
            where TReadModel : class, IReadModel<TBoundedContext>
        {
            var options = new ScyllaReadStoreOptions();
            configureOptions(options);

            services.AddSingleton(options);

            services.AddSingleton<ISession>(sp =>
            {
                var cluster = Cluster.Builder()
                    .AddContactPoints(options.ContactPoints)
                    .WithPort(options.Port)
                    .Build();

                var session = cluster.Connect();

                if (options.AutoCreateSchema)
                {
                    var logger = sp.GetRequiredService<ILogger<ScyllaSchemaInitializer>>();
                    var initializer = new ScyllaSchemaInitializer(options, logger);
                    initializer.InitializeAsync(session).GetAwaiter().GetResult();
                }

                return session;
            });

            services.AddScoped<IReadRepository<TBoundedContext, TReadModel>,
                ScyllaReadRepository<TBoundedContext, TReadModel>>();

            services.AddScoped<IReadModelStore<TBoundedContext, TReadModel>,
                ScyllaReadModelStore<TBoundedContext, TReadModel>>();

            return services;
        }
    }
}
