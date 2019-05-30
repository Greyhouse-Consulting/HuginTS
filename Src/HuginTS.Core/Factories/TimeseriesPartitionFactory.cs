using HuginTS.Core.Infrastructure;
using HuginTS.Core.Infrastructure.Contracts;
using MongoDB.Driver;

namespace HuginTS.Core.Factories
{
	public class TimeseriesPartitionFactory : ITimeseriesPartitionFactory
	{
		private readonly IMongoDatabase _mongoDatabase;

		public TimeseriesPartitionFactory(IMongoDatabase mongoDatabase)
		{
			_mongoDatabase = mongoDatabase;
		}

		public ITimeseriesPartitionRepository Create(string name)
		{
			return new TimeseriesPartitionRepository(_mongoDatabase, name);
		}
	}
}