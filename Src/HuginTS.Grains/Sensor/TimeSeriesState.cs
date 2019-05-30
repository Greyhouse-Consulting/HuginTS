using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace HuginTS.Grains.Sensor
{
    public class TimeSeriesState
    {
        public TimeSeriesState()
        {
            Partitions = new List<ObjectId>();
        }
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public DateTime Lastupdated { get; set; }

        public bool Created { get; set; }

        public IList<ObjectId> Partitions { get; set; }
    }
}