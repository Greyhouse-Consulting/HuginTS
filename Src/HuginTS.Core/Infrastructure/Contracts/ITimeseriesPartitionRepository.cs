using System;
using System.Threading.Tasks;
using HuginTS.Core.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HuginTS.Core.Infrastructure.Contracts
{
	public interface ITimeseriesPartitionRepository : IRepository<Partition>
	{
		Task UpdateAsync(ObjectId id, int x, int y, double value, FindOneAndUpdateOptions<Partition> options = null);
		Task<bool> ExistAsync(ObjectId id);

	}
}