namespace DigiTFactory.Libraries.ReadRepository.Postgres.Configuration
{
    /// <summary>
    /// Настройки PostgreSQL Read Store.
    /// </summary>
    public class PostgresReadStoreOptions
    {
        /// <summary>
        /// Строка подключения к PostgreSQL.
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Имя схемы для Read-моделей.
        /// </summary>
        public string SchemaName { get; set; } = "ReadModel";
    }
}
