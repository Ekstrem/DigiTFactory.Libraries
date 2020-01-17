using System;
using Microsoft.EntityFrameworkCore;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.TacticalPatterns.Repository;

namespace Hive.Dal
{
	/// <summary>
	/// Базовый репозиторий всех команд.
	/// </summary>
	/// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
	/// <typeparam name="TModel">Анемичная модель.</typeparam>
	public class CommandRepository<TBoundedContext, TModel> :
		BaseRepository, ICommandRepository<TBoundedContext, TModel>
		where TBoundedContext : IBoundedContext
		where TModel : AnemicModel<TBoundedContext>
	{
		/// <summary>
		/// Конструктор репозитория.
		/// </summary>
		/// <param name="context">Контекст подключения к базе данных.</param>
		public CommandRepository(DbContext context)
			: base(context)
		{
		}

		/// <summary>
		/// Добавляет запись в базу данных.
		/// </summary>
		/// <param name="entity">Сущность хранящаяся в базе данных.</param>
		/// <returns>Идентификатор сущности в базе даннх.</returns>
		public void Add(TModel entity)
		{
			GetSet<TModel>().Add(entity);
		}

		/// <summary>
		/// Обновление сущностей в базе данных.
		/// </summary>
		/// <param name="entity">Сущность для обновления в базе данных.</param>
		public void Update(TModel entity)
		{
			if (_context.Entry(entity).State == EntityState.Detached)
			{
				GetSet<TModel>().Attach(entity);
			}

			GetSet<TModel>().Update(entity);
		}

		/// <summary>
		/// Удаление записи из базы данных.
		/// </summary>
		/// <param name="entity">Сущность для удаления из базы данных.</param>
		public void Delete(TModel entity)
		{
			if (_context.Entry(entity).State == EntityState.Detached)
			{
				GetSet<TModel>().Attach(entity);
			}

			GetSet<TModel>().Remove(entity);
		}
	}
}
