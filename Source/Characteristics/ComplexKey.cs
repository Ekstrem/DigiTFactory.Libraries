using System;
using Hive.SeedWorks.Events;

namespace Hive.SeedWorks.Characteristics
{
    /// <summary>
    /// Состовной ключ.
    /// </summary>
    public sealed class ComplexKey : IHasComplexKey
    {
        private Guid _id;
        private long _version;

        private ComplexKey(Guid id, long version)
        {
            _id = id;
            _version = version;
        }

        /// <summary>
        /// Идентификатор сущности.
        /// </summary>
        public Guid Id => _id;

        /// <summary>
        /// Определяет версию. Ожидаемое использование - дата создания версии в милисекундах.
        /// Является приведением <see cref="DateTimeOffset"/> к формату времени
        /// Unix в милисекундах.
        /// </summary>
        public long Version => _version;

        /// <summary>
        /// Создание экземляра комплексного ключа.
        /// </summary>
        /// <param name="id">Идентификатор агрегата.</param>
        /// <param name="version">Версия агрегата.</param>
        /// <param name="correlationToken">Токен корреляции.</param>
        /// <returns></returns>
        public static ComplexKey Create(Guid id, long version)
            => new ComplexKey(id, version);
    }
}

