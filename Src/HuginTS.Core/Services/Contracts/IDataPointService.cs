using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuginTS.Core.Domain.Entities;
using MongoDB.Bson;

namespace HuginTS.Core.Services.Contracts
{
    public interface IDataPointService
    {
		Task<IList<Datapoint>> GetByPeriodAsync(string id, DateTime start, DateTime end);

		Task RegisterAsync(string timeseriesName, DateTime timeStamp, double value, bool createPartition = false);
    }
}