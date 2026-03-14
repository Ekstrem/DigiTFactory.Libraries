#nullable enable
using System;
using DigiTFactory.Libraries.CommandRepository.Mongo.Configuration;
using DigiTFactory.Libraries.CommandRepository.Mongo.Repositories;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DigiTFactory.Libraries.CommandRepository.Mongo.Extensions
{
    /// <summary>
    /// Расширения для регистрации MongoDB Event Store в DI контейнере.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрирует Event Store с MongoDB хранилищем.
        /// </summary>
        public static IServiceCollection AddEventStoreMongo<TBoundedContext, TAnemicModel>(
            this IServiceCollection services,
            Action<MongoEventStoreOptions> configureOptions)
            where TBoundedContext : IBoundedContext
            where TAnemicModel : IAnemicModel<TBoundedContext>
        {
            var options = new MongoEventStoreOptions();
            configureOptions(options);

            services.AddSingleton(options);
            services.AddSingleton<IMongoClient>(new MongoClient(options.ConnectionString));
            services.AddSingleton<MongoEventStoreContext>();

            switch (options.Strategy)
            {
                case EventStoreStrategy.FullEventSourcing:
                    services.AddScoped<
                        IMongoEventStoreRepository<TBoundedContext, TAnemicModel>,
                        MongoFullEventSourcingRepository<TBoundedContext, TAnemicModel>>();
                    break;

                case EventStoreStrategy.SnapshotAfterN:
                    services.AddScoped<
                        IMongoEventStoreRepository<TBoundedContext, TAnemicModel>,
                        MongoSnapshotRepository<TBoundedContext, TAnemicModel>>();
                    break;

                case EventStoreStrategy.StateOnly:
                    services.AddScoped<
                        IMongoEventStoreRepository<TBoundedContext, TAnemicModel>,
                        MongoStateOnlyRepository<TBoundedContext, TAnemicModel>>();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(options.Strategy), options.Strategy, "Unknown EventStoreStrategy");
            }

            // Обратная совместимость с SeedWorks
            services.AddScoped<IAnemicModelRepository<TBoundedContext, TAnemicModel>>(sp =>
                sp.GetRequiredService<IMongoEventStoreRepository<TBoundedContext, TAnemicModel>>());

            return services;
        }
    }
}
