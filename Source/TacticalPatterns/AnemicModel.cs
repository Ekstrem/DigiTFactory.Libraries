using System;
using System.Collections.Generic;
using DigiTFactory.Libraries.SeedWorks.Characteristics;
using DigiTFactory.Libraries.SeedWorks.Definition;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Базовый абстрактный класс анемичной модели.
    /// Предоставляет реализацию по умолчанию для <see cref="IAnemicModel{TBoundedContext}"/>.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public abstract class AnemicModel<TBoundedContext> : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly Guid _id;
        private readonly long _version;
        private readonly Guid _correlationToken;
        private readonly IDictionary<string, IValueObject> _invariants;

        /// <summary>
        /// Создание анемичной модели из комплексного ключа и набора объект-значений.
        /// </summary>
        /// <param name="key">Комплексный ключ агрегата.</param>
        /// <param name="invariants">Словарь объект-значений.</param>
        protected AnemicModel(IComplexKey key, IDictionary<string, IValueObject> invariants)
        {
            _id = key.Id;
            _version = key.Version;
            _correlationToken = key.CorrelationToken;
            _invariants = invariants ?? new Dictionary<string, IValueObject>();
        }

        /// <inheritdoc />
        public Guid Id => _id;

        /// <inheritdoc />
        public long Version => _version;

        /// <inheritdoc />
        public Guid CorrelationToken => _correlationToken;

        /// <summary>
        /// Имя команды (по умолчанию пустая строка).
        /// </summary>
        public virtual string CommandName => string.Empty;

        /// <summary>
        /// Имя субъекта бизнес-операции (по умолчанию пустая строка).
        /// </summary>
        public virtual string SubjectName => string.Empty;

        /// <summary>
        /// Словарь объект-значений (инвариантов) модели.
        /// </summary>
        public IDictionary<string, IValueObject> Invariants => _invariants;

        /// <inheritdoc />
        public IDictionary<string, IValueObject> GetValueObjects() => _invariants;
    }
}
