using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HuginTS.Core.Infrastructure.Contracts
{
	public interface IRepository<T> where T : class
	{
		Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
		Task InsertAsync(T t);
		Task UpdateAsync(T t);

		Task<IList<T>> WhereAsync(Expression<Func<T, bool>> filter);

		Task<IList<T>> AllAsync();
	}
}