using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HuginTS.Core.Infrastructure.Contracts;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HuginTS.Core.Infrastructure
{
	public interface IRepository
	{
		Task GetAsync(ObjectId id);
	}

	public abstract class Repository<T> : IRepository<T>, IRepository where T : class, IIdentifier
	{
	    private readonly IMongoDatabase _database;
		protected readonly IMongoCollection<T> _collection;

		protected Repository(IMongoDatabase database, string collectionName)
		{
			_database = database;
			_collection = _database.GetCollection<T>(collectionName);
		}

		public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
		{
            return await _collection.Find(filter).FirstOrDefaultAsync();
		}

		public virtual async Task<IList<T>> WhereAsync(Expression<Func<T, bool>> filter)
		{
			return await _collection.Find(filter).ToListAsync();
		}

		public virtual async Task<IList<T>> AllAsync()
		{
			return await _collection.Find(_ => true).ToListAsync();
		}

		public virtual async Task InsertAsync(T t)
		{
			await _collection.InsertOneAsync(t);
		}

		public virtual async Task GetAsync(ObjectId id)
		{
			await FirstOrDefaultAsync(t => t.Id == id);
		}
		public virtual async Task UpdateAsync(T t)
		{
			var filter = Builders<T>.Filter.Eq(s => s.Id, t.Id);

			await _collection.ReplaceOneAsync(filter, t);
		}
	}
}