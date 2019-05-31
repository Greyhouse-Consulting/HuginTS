using System;
using System.Collections.Generic;
using HuginTS.Core.Infrastructure;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HuginTS.Core.Domain.Entities
{
    public class Partition : IIdentifier
    {
        public Partition(int x, int y)
        {
            Datapoints = new double?[x, y];
        }

        public ObjectId Id { get; set; }
        public ObjectId TimeSeriesId { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime StartOfPeriod { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Created { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Modified { get; set; }

        public double?[,] Datapoints { get; set; }


        public IEnumerable<Datapoint> Expand()
        {
            for (var x = 0; x < Datapoints.GetLength(0); x++)
            {
                for (var y = 0; y < Datapoints.GetLength(1); y++)
                {
                    if (Datapoints[x, y].HasValue)
                    {
                        yield return new Datapoint(new DateTime(StartOfPeriod.Year, StartOfPeriod.Month, StartOfPeriod.Day, x / 60, x % 60, y),
                            Datapoints[x, y].Value
                        );
                    }
                }
            }
        }
    }
}