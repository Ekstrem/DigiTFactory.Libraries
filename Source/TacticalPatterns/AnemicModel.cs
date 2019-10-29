using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Hive.SeedWorks.LifeCircle
{
    /// <summary>
    /// Базовый класс анемичной модели.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public class AnemicModel<TBoundedContext>
        : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly IAggregateRoot<TBoundedContext> _root;
        private readonly IDictionary<string, IValueObject> _valueObjects;

        /// <summary>
        /// Конструктор анемичной модели.
        /// Потомки должны реализовать свой конструктор с валидацией объект значений.
        /// </summary>
        /// <param name="root">Корень агрегации.</param>
        /// <param name="valueObjects">Словарь объект значений.</param>
        public AnemicModel(
            IAggregateRoot<TBoundedContext> root,
            IDictionary<string, IValueObject> valueObjects)
        {
            _root = root;
            _valueObjects = valueObjects;
        }

        /// <summary>
        /// Конструктор анемичной модели.
        /// Потомки должны реализовать свой конструктор с валидацией объект значений.
        /// </summary>
        /// <param name="model">Анемичная модель бизнес-объекта.</param>
        public AnemicModel(IAnemicModel<TBoundedContext> model)
            : this(model.Root, GetValueObjects(model))
        {

        }

        private static IDictionary<string, IValueObject> GetValueObjects(IAnemicModel<TBoundedContext> model)
        {
            var valueObjects = model.ValueObjects;
            var minedValueObjects = model.GetFields();
            foreach (var key in minedValueObjects.Keys.Except(valueObjects.Keys))
            {
                valueObjects.TryAdd(key, minedValueObjects[key]);
            }

            return valueObjects;
        }

        /// <summary>
        /// Конструктор анемичной модели.
        /// Потомки должны реализовать свой конструктор с валидацией объект значений.
        /// </summary>
        /// <param name="root">Корень агрегации.</param>
        /// <param name="valueObjects">Словарь объект значений.</param>
        protected AnemicModel(
            IAggregateRoot<TBoundedContext> root,
            params IValueObject[] valueObjects)
            : this(root, valueObjects.ToImmutableDictionary(k => k.GetType().Name, v => v))
        {
        }

        /// <summary>
        /// Имеет родителя.
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// Имя ограниченного контекста.
        /// </summary>
        protected string ContextName => typeof(TBoundedContext).Name;

        /// <summary>
        /// Корень модели сущности.
        /// </summary>
        public IAggregateRoot<TBoundedContext> Root => _root;

        /// <summary>
        /// Словарь объект значений.
        /// </summary>
        public IDictionary<string, IValueObject> ValueObjects => _valueObjects;
    }
}