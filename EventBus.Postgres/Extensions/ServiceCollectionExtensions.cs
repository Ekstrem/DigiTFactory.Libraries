using System;
using DigiTFactory.Libraries.EventBus.Postgres.Configuration;
using DigiTFactory.Libraries.SeedWorks.Events;
using Microsoft.Extensions.DependencyInjection;

namespace DigiTFactory.Libraries.EventBus.Postgres.Extensions
{
    /// <summary>
    /// Расширения для регистрации PostgreSQL EventBus (outbox) в DI контейнере.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрирует PostgreSQL (outbox) реализацию IEventBus.
        /// </summary>
        public static IServiceCollection AddEventBusPostgres(
            this IServiceCollection services,
            Action<PostgresEventBusOptions> configureOptions)
        {
            var options = new PostgresEventBusOptions();
            configureOptions(options);

            services.AddSingleton(options);
            services.AddSingleton<PostgresOutboxInitializer>();
            services.AddSingleton<PostgresEventBusProducer>();
            services.AddSingleton<PostgresEventBusConsumer>();

            services.AddSingleton<PostgresEventBus>(sp => new PostgresEventBus(
                sp.GetRequiredService<PostgresEventBusProducer>(),
                sp.GetRequiredService<PostgresEventBusConsumer>()));

            services.AddSingleton<IEventBus>(sp => sp.GetRequiredService<PostgresEventBus>());
            services.AddSingleton<IEventBusProducer>(sp => sp.GetRequiredService<PostgresEventBus>());
            services.AddSingleton<IEventBusConsumer>(sp => sp.GetRequiredService<PostgresEventBus>());

            // PostgresEventBusConsumer is IHostedService — register for auto-start
            services.AddHostedService(sp => sp.GetRequiredService<PostgresEventBusConsumer>());

            return services;
        }
    }
}
