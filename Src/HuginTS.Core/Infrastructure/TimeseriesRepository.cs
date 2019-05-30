using System;
using System.Threading.Tasks;
using HuginTS.Core.Domain.Entities;
using HuginTS.Core.Infrastructure.Contracts;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HuginTS.Core.Infrastructure
{
	public class TimeseriesRepository : Repository<TimeSerie>, ITimeseriesRepository
	{
		public TimeseriesRepository(IMongoDatabase database) : base(database, "TimeSeries")  { }
		public async Task<bool> ExistsAsync(ObjectId id)
		{
			return await _collection.CountDocumentsAsync(t => t.Id == id) > 0;
		}
	}
}