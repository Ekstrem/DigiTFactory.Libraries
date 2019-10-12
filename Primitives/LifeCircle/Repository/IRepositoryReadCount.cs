using System.Threading;
using System.Threading.Tasks;
using Hive.SeedWorks.Pipelines.Abstractions;
using Hive.SeedWorks.Specification;

namespace Hive.SeedWorks.LifeCircle.Repository
{
	/// <summary>
	/// Интерфейс репозитория. Функциональность чтения.
	/// </summary>
	/// <typeparam name="TEntity">Тип записи.</typeparam>
	public interface IRepositoryReadCount<TEntity>
		where TEntity : class, IEntity
	{
		/// <summary>
		/// Получить общее количество элементов.
		/// </summary>
		/// <returns>Возвращает количество элементов данных.</returns>
		long Count();

		/// <summary>
		/// Получить общее количество элементов.
		/// </summary>
		/// <param name="value">Значение для поиска по спецификации.</param>
		/// <param name="specification">Условие выборки.</param>
		/// <typeparam name="TIn">Тип входная Dto фильтрации.</typeparam>
		/// <returns>Возвращает количество элементов данных.</returns>
		long Count<TIn>(
			TIn value,
			IQuerySpecification<TIn, TEntity> specification)
			where TIn : IQuery;

		/// <summary>
		/// Получить общее количество элементов.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Возвращает количество элементов данных.</returns>
		Task<long> CountAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Получить общее количество элементов.
		/// </summary>
		/// <param name="value">Значение для поиска по спецификации.</param>
		/// <param name="specification">Условие выборки.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <typeparam name="TIn">Тип входная Dto фильтрации.</typeparam>
		/// <returns>Возвращает количество элементов данных.</returns>
		Task<long> CountAsync<TIn>(
			TIn value,
			IQuerySpecification<TIn, TEntity> specification,
			CancellationToken cancellationToken = default)
			where TIn : IQuery;
	}

	/// <summary>
	/// Интерфейс репозитория. Функциональность чтения.
	/// </summary>
	public interface IRepositoryReadCount
	{
		/// <summary>
		/// Получить общее количество элементов.
		/// </summary>
		/// <typeparam name="TEntity">Тип записи.</typeparam>
		/// <returns>Возвращает количество элементов данных.</returns>
		long Count<TEntity>()
			where  TEntity : class, IEntity;

		/// <summary>
		/// Получить общее количество элементов.
		/// </summary>
		/// <param name="value">Значение для поиска по спецификации.</param>
		/// <param name="specification">Условие выборки.</param>
		/// <typeparam name="TEntity">Тип записи.</typeparam>
		/// <typeparam name="TIn">Тип входная Dto фильтрации.</typeparam>
		/// <returns>Возвращает количество элементов данных.</returns>
		long Count<TEntity, TIn>(
			TIn value,
			IQuerySpecification<TIn, TEntity> specification)
			where TEntity : class, IEntity
			where TIn : IQuery;

		/// <summary>
		/// Получить общее количество элементов.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <typeparam name="TEntity">Тип записи.</typeparam>
		/// <returns>Возвращает количество элементов данных.</returns>
		Task<long> CountAsync<TEntity>(CancellationToken cancellationToken = default)
			where TEntity : class, IEntity;

		/// <summary>
		/// Получить общее количество элементов.
		/// </summary>
		/// <param name="value">Значение для поиска по спецификации.</param>
		/// <param name="specification">Условие выборки.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <typeparam name="TEntity">Тип записи.</typeparam>
		/// <typeparam name="TIn">Тип входная Dto фильтрации.</typeparam>
		/// <returns>Возвращает количество элементов данных.</returns>
		Task<long> CountAsync<TEntity, TIn>(
			TIn value,
			IQuerySpecification<TIn, TEntity> specification,
			CancellationToken cancellationToken = default)
			where TEntity : class, IEntity
			where TIn : IQuery;
	}
}
