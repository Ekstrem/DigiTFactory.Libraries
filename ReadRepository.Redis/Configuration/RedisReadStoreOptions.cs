using System;

namespace DigiTFactory.Libraries.ReadRepository.Redis.Configuration
{
    /// <summary>
    /// Настройки Redis Read Store.
    /// </summary>
    public class RedisReadStoreOptions
    {
        /// <summary>
        /// Строка подключения к Redis (формат StackExchange.Redis).
        /// </summary>
        public string ConnectionString { get; set; } = "localhost:6379";

        /// <summary>
        /// Префикс ключей для Read-моделей.
        /// </summary>
        public string KeyPrefix { get; set; } = "read:";

        /// <summary>
        /// Время жизни записей (null = без ограничений).
        /// </summary>
        public TimeSpan? DefaultTtl { get; set; }
    }
}
