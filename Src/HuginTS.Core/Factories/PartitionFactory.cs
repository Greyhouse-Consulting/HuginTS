using System;
using System.Collections.Generic;
using HuginTS.Core.Domain.Entities;
using MongoDB.Bson;

namespace HuginTS.Core.Factories
{
    public class PartitionFactory
	{


		private static readonly IDictionary<PartitionGranularity, PartitionMatrixSize> PartitionMatrixSizes = new Dictionary<PartitionGranularity, PartitionMatrixSize>
		{
			{ PartitionGranularity.Second, new PartitionMatrixSize(1440, 60)  },
			{ PartitionGranularity.Minute, new PartitionMatrixSize(24, 60)  },
			{ PartitionGranularity.Quarter, new PartitionMatrixSize(24, 15)  },
			{ PartitionGranularity.Hour, new PartitionMatrixSize(24, 1)  },
		};

		public static Partition Create(PartitionGranularity granularity, ObjectId id, DateTime timeStamp)
		{

			if (granularity != PartitionGranularity.Second)
				throw new NotImplementedException("Only second supported at the moment");

			return new Partition(PartitionMatrixSizes[granularity].X, PartitionMatrixSizes[granularity].Y)
			{
				Id = id,
				StartOfPeriod = new DateTime(timeStamp.Year, timeStamp.Month, timeStamp.Day),
				Created = DateTime.Now
			};
		}
	}
}