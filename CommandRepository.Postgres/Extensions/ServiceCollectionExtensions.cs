#nullable enable
using System;
using DigiTFactory.Libraries.CommandRepository.Postgres.Configuration;
using DigiTFactory.Libraries.CommandRepository.Postgres.Repositories;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigiTFactory.Libraries.CommandRepository.Postgres.Extensions
{
    /// <summary>
    /// Расширения для регистрации Event Store в DI контейнере.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрирует Event Store с PostgreSQL хранилищем.
        /// Стратегия задаётся через <paramref name="configureOptions"/>.
        /// </summary>
        /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
        /// <typeparam name="TAnemicModel">Тип анемичной модели.</typeparam>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="connectionString">Строка подключения к PostgreSQL.</param>
        /// <param name="configureOptions">Настройки Event Store (стратегия, интервал snapshot, схема).</param>
        /// <returns>Коллекция сервисов.</returns>
        public static IServiceCollection AddEventStorePostgres<TBoundedContext, TAnemicModel>(
            this IServiceCollection services,
            string connectionString,
            Action<EventStoreOptions>? configureOptions = null)
            where TBoundedContext : IBoundedContext
            where TAnemicModel : IAnemicModel<TBoundedContext>
        {
            var options = new EventStoreOptions();
            configureOptions?.Invoke(options);

            services.AddSingleton(options);

            services.AddDbContext<EventStoreDbContext>(dbOptions =>
                dbOptions.UseNpgsql(connectionString, npgsql =>
                    npgsql.MigrationsAssembly(typeof(EventStoreDbContext).Assembly.GetName().Name)));

            switch (options.Strategy)
            {
                case EventStoreStrategy.FullEventSourcing:
                    services.AddScoped<
                        IEventStoreRepository<TBoundedContext, TAnemicModel>,
                        FullEventSourcingRepository<TBoundedContext, TAnemicModel>>();
                    break;

                case EventStoreStrategy.SnapshotAfterN:
                    services.AddScoped<
                        IEventStoreRepository<TBoundedContext, TAnemicModel>,
                        SnapshotRepository<TBoundedContext, TAnemicModel>>();
                    break;

                case EventStoreStrategy.StateOnly:
                    services.AddScoped<
                        IEventStoreRepository<TBoundedContext, TAnemicModel>,
                        StateOnlyRepository<TBoundedContext, TAnemicModel>>();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(options.Strategy),
                        options.Strategy,
                        "Unknown EventStoreStrategy");
            }

            // Регистрация IAnemicModelRepository для обратной совместимости с SeedWorks
            services.AddScoped<IAnemicModelRepository<TBoundedContext, TAnemicModel>>(sp =>
                sp.GetRequiredService<IEventStoreRepository<TBoundedContext, TAnemicModel>>());

            return services;
        }
    }
}
