using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuginTS.Core;
using HuginTS.Core.Domain.Entities;
using HuginTS.Core.Models;
using HuginTS.Core.Services.Contracts;
using HuginTS.Grain.Contracts;
using MongoDB.Bson;
using Orleans;

namespace HuginTS.Grains.Sensor
{
    public class TimeSeriesGrain : Grain<TimeSeriesState>, ITimeSeriesGrain
    {
        private readonly IDataPointService _dataPointService;

        public TimeSeriesGrain(IDataPointService dataPointService)
        {
            _dataPointService = dataPointService;
        }

        public async Task Add(DatapointModel datapoint)
        {
            await EnsureCreatedAsync(datapoint.Name);

            var partitionId = IdGenerator.CreateObjectId(datapoint.Timestamp);
            var partitionExists = State?.Partitions?.Contains(partitionId) ?? false;

            await _dataPointService.RegisterAsync(datapoint.Name, datapoint.Timestamp, datapoint.Value, partitionExists);

            if (!partitionExists)
            {
                if(State.Partitions == null)
                    State.Partitions = new List<ObjectId>();

                State.Partitions.Add(partitionId);
            }

            State.Lastupdated = DateTime.Now;
            await WriteStateAsync();
        }

        private async Task EnsureCreatedAsync(string timeseriesName)
        {
            if (!State.Created)
            {
                State.Created = true;
                State.Name = timeseriesName;
                await WriteStateAsync();
            }
        }
    }
}
