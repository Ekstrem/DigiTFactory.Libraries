using System;
using DigiTFactory.Libraries.AbstractAggregate.Cache;
using DigiTFactory.Libraries.AbstractAggregate.Factory;
using DigiTFactory.Libraries.SeedWorks.Definition;
using Microsoft.Extensions.DependencyInjection;

namespace DigiTFactory.Libraries.AbstractAggregate.Extensions
{
    /// <summary>
    /// Методы расширения для регистрации AbstractAggregate в DI-контейнере.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Зарегистрировать ядро AbstractAggregate: фабрику и кэш метаданных.
        /// </summary>
        /// <typeparam name="TBoundedContext">Ограниченный контекст микросервиса.</typeparam>
        /// <param name="services">Коллекция сервисов.</param>
        /// <returns>Коллекция сервисов для chaining.</returns>
        /// <remarks>
        /// Необходимо также зарегистрировать IMetadataRepository
        /// (например, через AddAbstractAggregatePostgres).
        /// </remarks>
        public static IServiceCollection AddAbstractAggregate<TBoundedContext>(
            this IServiceCollection services)
            where TBoundedContext : IBoundedContext
        {
            services.AddSingleton<IMetadataCache, InMemoryMetadataCache>();
            services.AddScoped<AbstractAggregateFactory<TBoundedContext>>();

            return services;
        }
    }
}
