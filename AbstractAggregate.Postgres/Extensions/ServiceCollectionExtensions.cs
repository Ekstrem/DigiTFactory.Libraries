using System;
using DigiTFactory.Libraries.AbstractAggregate.Postgres.Repositories;
using DigiTFactory.Libraries.AbstractAggregate.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigiTFactory.Libraries.AbstractAggregate.Postgres.Extensions
{
    /// <summary>
    /// Методы расширения для регистрации PostgreSQL metadata store.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Зарегистрировать PostgreSQL реализацию IMetadataRepository.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="connectionString">Строка подключения к PostgreSQL.</param>
        /// <returns>Коллекция сервисов для chaining.</returns>
        public static IServiceCollection AddAbstractAggregatePostgres(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<MetadataDbContext>(options =>
                options.UseNpgsql(connectionString,
                    npgsql => npgsql.MigrationsAssembly(
                        typeof(MetadataDbContext).Assembly.FullName)));

            services.AddScoped<IMetadataRepository, PostgresMetadataRepository>();

            return services;
        }
    }
}
