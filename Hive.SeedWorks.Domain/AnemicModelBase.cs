using Hive.SeedWorks.LifeCircle;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Hive.SeedWorks.Domain
{
    /// <summary>
    /// Базовый класс анемичной модели.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    /// <typeparam name="TKey">Ключевое поле.</typeparam>
    public abstract class AnemicModelBase<TBoundedContext, TKey>
        : IAnemicModel<TBoundedContext, TKey>
        where TBoundedContext : IBoundedContext
    {
        private readonly IAggregateRoot<TBoundedContext, TKey> _root;
        private readonly IDictionary<string, IValueObject> _valueObjects;

        /// <summary>
        /// Конструктор анемичной модели.
        /// Потомки должны реализовать свой конструктор с валидацией объект значений.
        /// </summary>
        /// <param name="root">Корень агрегации.</param>
        /// <param name="valueObjects">Словарь объект значений.</param>
        protected AnemicModelBase(
            IAggregateRoot<TBoundedContext, TKey> root,
            IDictionary<string, IValueObject> valueObjects)
        {
            _root = root;
            _valueObjects = valueObjects;
        }

        /// <summary>
        /// Конструктор анемичной модели.
        /// Потомки должны реализовать свой конструктор с валидацией объект значений.
        /// </summary>
        /// <param name="root">Корень агрегации.</param>
        /// <param name="valueObjects">Словарь объект значений.</param>
        protected AnemicModelBase(
            IAggregateRoot<TBoundedContext, TKey> root,
            params IValueObject[] valueObjects)
            : this(root, valueObjects.ToImmutableDictionary(k => k.GetType().Name, v => v))
        {
        }

        /// <summary>
        /// Имеет родителя.
        /// </summary>
        public TKey Id { get; protected set; }

        /// <summary>
        /// Имя ограниченного контекста.
        /// </summary>
        protected string ContextName => typeof(TBoundedContext).Name;

        /// <summary>
        /// Корень модели сущности.
        /// </summary>
        public IAggregateRoot<TBoundedContext, TKey> Root => _root;

        /// <summary>
        /// Словарь объект значений.
        /// </summary>
        public IDictionary<string, IValueObject> ValueObjects => _valueObjects;
    }


    /// <summary>
    /// Базовый класс анемичной модели.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public abstract class AnemicModelBase<TBoundedContext>
        : AnemicModelBase<TBoundedContext, Guid>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Конструктор анемичной модели.
        /// Потомки должны реализовать свой конструктор с валидацией объект значений.
        /// </summary>
        /// <param name="root">Корень агрегации.</param>
        /// <param name="valueObjects">Словарь объект значений.</param>
        protected AnemicModelBase(
            IAggregateRoot<TBoundedContext, Guid> root,
            IDictionary<string, IValueObject> valueObjects)
            : base(root, valueObjects)
        {
        }

        /// <summary>
        /// Конструктор анемичной модели.
        /// Потомки должны реализовать свой конструктор с валидацией объект значений.
        /// </summary>
        /// <param name="root">Корень агрегации.</param>
        /// <param name="valueObjects">Словарь объект значений.</param>
        protected AnemicModelBase(
            IAggregateRoot<TBoundedContext, Guid> root,
            params IValueObject[] valueObjects)
            : base(root, valueObjects)
        {
        }
    }
}