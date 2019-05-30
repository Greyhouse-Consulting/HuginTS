using System;
using System.Threading.Tasks;
using HuginTS.Core.Domain.Entities;
using MongoDB.Bson;

namespace HuginTS.Core.Infrastructure.Contracts
{
	public interface ITimeseriesRepository : IRepository<TimeSerie>
	{
		Task<bool> ExistsAsync(ObjectId id);
	}
}