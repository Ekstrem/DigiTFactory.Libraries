using System;
using DigiTFactory.Libraries.ReadRepository.Postgres.Configuration;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigiTFactory.Libraries.ReadRepository.Postgres.Extensions
{
    /// <summary>
    /// Расширения для регистрации PostgreSQL Read Store в DI контейнере.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрирует PostgreSQL Read Store с указанным DbContext.
        /// </summary>
        /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
        /// <typeparam name="TReadModel">Тип Read-модели.</typeparam>
        /// <typeparam name="TDbContext">Тип DbContext для Read-моделей.</typeparam>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="connectionString">Строка подключения к PostgreSQL.</param>
        /// <param name="configureOptions">Настройки Read Store.</param>
        /// <returns>Коллекция сервисов.</returns>
        public static IServiceCollection AddReadStorePostgres<TBoundedContext, TReadModel, TDbContext>(
            this IServiceCollection services,
            string connectionString,
            Action<PostgresReadStoreOptions> configureOptions = null)
            where TBoundedContext : IBoundedContext
            where TReadModel : class, IReadModel<TBoundedContext>
            where TDbContext : DbContext
        {
            var options = new PostgresReadStoreOptions();
            configureOptions?.Invoke(options);
            options.ConnectionString = connectionString;

            services.AddSingleton(options);

            services.AddDbContext<TDbContext>(dbOptions =>
                dbOptions.UseNpgsql(connectionString, npgsql =>
                    npgsql.MigrationsAssembly(typeof(TDbContext).Assembly.GetName().Name)));

            services.AddScoped<IReadRepository<TBoundedContext, TReadModel>,
                PostgresReadRepository<TBoundedContext, TReadModel>>(sp =>
                    new PostgresReadRepository<TBoundedContext, TReadModel>(
                        sp.GetRequiredService<TDbContext>()));

            services.AddScoped<IReadModelStore<TBoundedContext, TReadModel>,
                PostgresReadModelStore<TBoundedContext, TReadModel>>(sp =>
                    new PostgresReadModelStore<TBoundedContext, TReadModel>(
                        sp.GetRequiredService<TDbContext>()));

            return services;
        }
    }
}
