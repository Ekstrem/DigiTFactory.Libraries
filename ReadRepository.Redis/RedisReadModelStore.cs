using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.ReadRepository.Redis.Configuration;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace DigiTFactory.Libraries.ReadRepository.Redis
{
    /// <summary>
    /// Redis реализация IReadModelStore.
    /// Используется проекциями для записи/обновления Read-моделей в Redis.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    /// <typeparam name="TReadModel">Тип Read-модели.</typeparam>
    public class RedisReadModelStore<TBoundedContext, TReadModel>
        : IReadModelStore<TBoundedContext, TReadModel>
        where TBoundedContext : IBoundedContext
        where TReadModel : class, IReadModel<TBoundedContext>
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly RedisReadStoreOptions _options;
        private readonly ILogger<RedisReadModelStore<TBoundedContext, TReadModel>> _logger;
        private readonly string _keyPattern;

        public RedisReadModelStore(
            IConnectionMultiplexer redis,
            RedisReadStoreOptions options,
            ILogger<RedisReadModelStore<TBoundedContext, TReadModel>> logger)
        {
            _redis = redis ?? throw new ArgumentNullException(nameof(redis));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _keyPattern = $"{_options.KeyPrefix}{typeof(TReadModel).Name}:";
        }

        /// <inheritdoc />
        public async Task UpsertAsync(TReadModel model, CancellationToken cancellationToken = default)
        {
            var db = _redis.GetDatabase();
            var id = GetIdFromModel(model);
            var json = JsonSerializer.Serialize(model);

            await db.StringSetAsync(GetKey(id), json, _options.DefaultTtl);

            _logger.LogDebug("Upserted {ModelType} with id {Id}", typeof(TReadModel).Name, id);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(GetKey(id));

            _logger.LogDebug("Deleted {ModelType} with id {Id}", typeof(TReadModel).Name, id);
        }

        private string GetKey(Guid id) => $"{_keyPattern}{id}";

        private static Guid GetIdFromModel(TReadModel model)
        {
            var idProperty = typeof(TReadModel).GetProperty("Id");
            if (idProperty == null)
                throw new InvalidOperationException(
                    $"Read model {typeof(TReadModel).Name} must have an 'Id' property of type Guid.");

            return (Guid)idProperty.GetValue(model)!;
        }
    }
}
