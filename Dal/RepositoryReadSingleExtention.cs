using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hive.SeedWorks.TacticalPatterns.Repository;

namespace Hive.Dal
{
	public static class RepositoryReadSingleExtention
	{
		public static Task<TModel> ToSingleAsync<TModel>(
			this IQueryable<TModel> queryable,
			SingleReadMode mode = default,
			CancellationToken cancellationToken = default)
		{
			switch (mode)
			{
				case SingleReadMode.Single:
					return queryable.SingleOrDefaultAsync(cancellationToken);
				case SingleReadMode.First:
					return queryable.FirstOrDefaultAsync(cancellationToken);
				case SingleReadMode.Last:
					return queryable.LastOrDefaultAsync(cancellationToken);
				default:
					throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
			}
		}
		public static TModel ToSingle<TModel>(
			this IQueryable<TModel> queryable,
			SingleReadMode mode = default)
		{
			switch (mode)
			{
				case SingleReadMode.Single:
					return queryable.SingleOrDefault();
				case SingleReadMode.First:
					return queryable.FirstOrDefault();
				case SingleReadMode.Last:
					return queryable.LastOrDefault();
				default:
					throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
			}
		}
	}
}
