using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HuginTS.Core.Domain;
using HuginTS.Core.Domain.Entities;
using HuginTS.Core.Factories;
using HuginTS.Core.Services.Contracts;
using MongoDB.Bson;

namespace HuginTS.Core.Services
{

    public class DataPointService : IDataPointService
    {
        private readonly ITimeseriesPartitionFactory _timeseriesPartitionFactory;

        public DataPointService(ITimeseriesPartitionFactory timeseriesPartitionFactory)
        {
            _timeseriesPartitionFactory = timeseriesPartitionFactory;
        }

        public async Task RegisterAsync(string timeseriesName, DateTime timeStamp, double value, bool partitionExists = false )
        {
            var id = IdGenerator.CreateObjectId(timeStamp);

            var partitionRepository = _timeseriesPartitionFactory.Create(timeseriesName);

            const PartitionGranularity partitionGranularity = PartitionGranularity.Second; // TODO Only supporting second granylarity at the moment

            var position = PartitionMatrixOffsetCalculator.CalculatePosition(partitionGranularity, timeStamp);

            if (!partitionExists)
            {
                var partition = PartitionFactory.Create(partitionGranularity, id, timeStamp);

                partition.Datapoints[position.X, position.Y] = value;

                await partitionRepository.InsertAsync(partition);
            }
            else
            {
                await partitionRepository.UpdateAsync(id, position.X, position.Y, value);
            }
        }

        public async Task<IList<Datapoint>> GetByPeriodAsync(string timeseriesId, DateTime start, DateTime end)
        {
            var startOfPeriod = new DateTime(start.Year, start.Month, start.Day);

            var endOfPeriod = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);

            var partitionRepository = _timeseriesPartitionFactory.Create(timeseriesId);

            var periodPartitions = await partitionRepository.WhereAsync(p => p.StartOfPeriod >= startOfPeriod && p.StartOfPeriod <= endOfPeriod);

            return CreateTimeserie(start, end, periodPartitions);
        }


        private List<Datapoint> CreateTimeserie(DateTime start, DateTime end, IList<Partition> periodPartitions)
        {
            var list = new List<Datapoint>();

            foreach (var periodPartition in periodPartitions)
            {
                var collection = periodPartition.Expand();

                if (SameShortDate(start, periodPartition))
                {
                    collection = collection.Where(dp => dp.Timestamp >= start);
                }

                if (SameShortDate(end, periodPartition))
                {
                    collection = collection.Where(dp => dp.Timestamp <= end);
                }

                list.AddRange(collection);
            }
            return list;
        }

        private static bool SameShortDate(DateTime date, Partition periodPartition)
        {
            return periodPartition.StartOfPeriod.Year == date.Year &&
                   periodPartition.StartOfPeriod.Month == date.Month &&
                   periodPartition.StartOfPeriod.Day == date.Day;
        }
    }
}
