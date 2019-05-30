using System;
using System.Threading.Tasks;
using HuginTS.Core.Domain.Entities;
using HuginTS.Core.Infrastructure.Contracts;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HuginTS.Core.Infrastructure
{
	public class TimeseriesPartitionRepository : Repository<Partition>, ITimeseriesPartitionRepository
	{

		public TimeseriesPartitionRepository(IMongoDatabase database, string collectionName) : base(database, collectionName) { }

		public async Task UpdateAsync(ObjectId id, int x, int y, double value, FindOneAndUpdateOptions<Partition> options = null )
		{
			var filter = Builders<Partition>.Filter.Eq(f => f.Id, id);

			FieldDefinition<Partition, double> field = $"Datapoints.{x}.{y}";

			var update = Builders<Partition>.Update
						.Set(field, value)
						.Set(p => p.Modified, DateTime.Now);

			await _collection.FindOneAndUpdateAsync(filter, update, options);
		}

        public async Task<bool> ExistAsync(ObjectId id)
		{
			return await _collection.Find(f => f.Id == id).CountDocumentsAsync() > 0;
		}
	}

}