using System.Collections.Generic;
using Hive.SeedWorks.Pipelines.Abstractions;
using Hive.SeedWorks.Specification;

namespace Hive.SeedWorks.LifeCircle.Repository
{
	/// <summary>
	/// Интерфейс репозитория. Функциональность чтения.
	/// </summary>
	/// <typeparam name="TEntity">Тип записи.</typeparam>
	public interface IRepositoryReadEnumerable<TEntity>
		where TEntity : class, IEntity
	{
		/// <summary>
		/// Получение всего набора записей.
		/// </summary>
		/// <returns>Набор записей.</returns>
		IEnumerable<TEntity> Get(IPaging page);

		/// <summary>
		/// Получение записи по спецификации.
		/// </summary>
		/// <param name="value">Значение для поиска по спецификации.</param>
		/// <param name="specification">Спецификация запроса.</param>
		/// <param name="sorter">Условие сортировки.</param>
		/// <param name="paging">Паджинация запроса.</param>
		/// <typeparam name="TIn">Тип входная Dto поиска.</typeparam>
		/// <returns>Коллекция агрегатов.</returns>
		IEnumerable<TEntity> Get<TIn>(
			TIn value,
			IQuerySpecification<TIn, TEntity> specification,
			IOrderSpecification<TEntity> sorter = default,
			IPaging paging = default)
			where TIn : IQuery;

		/// <summary>
		/// Получение записи по спецификации.
		/// </summary>
		/// <param name="value">Значение для поиска по спецификации.</param>
		/// <param name="specification">Спецификация запроса.</param>
		/// <param name="sorter">Условие сортировки.</param>
		/// <param name="paging">Паджинация запроса.</param>
		/// <typeparam name="TIn">Тип входная Dto поиска.</typeparam>
		/// <returns>Коллекция сущностей.</returns>
		IAsyncEnumerable<TEntity> GetReactive<TIn>(
			TIn value,
			IQuerySpecification<TIn, TEntity> specification,
			IOrderSpecification<TEntity> sorter = default,
			IPaging paging = default)
			where TIn : IQuery;
	}

	/// <summary>
	/// Интерфейс репозитория. Функциональность чтения.
	/// </summary>
	public interface IRepositoryReadEnumerable
	{
		/// <summary>
		/// Получение всего набора записей.
		/// </summary>
		/// <typeparam name="TEntity">Тип записи.</typeparam>
		/// <returns>Набор записей.</returns>
		IEnumerable<TEntity> Get<TEntity>(IPaging page)
			where TEntity : class, IEntity;

		/// <summary>
		/// Получение записи по спецификации.
		/// </summary>
		/// <param name="value">Значение для поиска по спецификации.</param>
		/// <param name="specification">Спецификация запроса.</param>
		/// <param name="sorter">Условие сортировки.</param>
		/// <param name="paging">Паджинация запроса.</param>
		/// <typeparam name="TEntity">Тип записи.</typeparam>
		/// <typeparam name="TIn">Тип входная Dto поиска.</typeparam>
		/// <returns>Коллекция агрегатов.</returns>
		IEnumerable<TEntity> Get<TEntity, TIn>(
			TIn value,
			IQuerySpecification<TIn, TEntity> specification,
			IOrderSpecification<TEntity> sorter = default,
			IPaging paging = default)
			where  TEntity : class, IEntity
			where TIn : IQuery;

		/// <summary>
		/// Получение записи по спецификации.
		/// </summary>
		/// <param name="value">Значение для поиска по спецификации.</param>
		/// <param name="specification">Спецификация запроса.</param>
		/// <param name="sorter">Условие сортировки.</param>
		/// <param name="paging">Паджинация запроса.</param>
		/// <typeparam name="TEntity">Тип записи.</typeparam>
		/// <typeparam name="TIn">Тип входная Dto поиска.</typeparam>
		/// <returns>Коллекция сущностей.</returns>
		IAsyncEnumerable<TEntity> GetReactive<TEntity, TIn>(
			TIn value,
			IQuerySpecification<TIn, TEntity> specification,
			IOrderSpecification<TEntity> sorter = default,
			IPaging paging = default)
			where  TEntity : class, IEntity
			where TIn : IQuery;
	}
}
