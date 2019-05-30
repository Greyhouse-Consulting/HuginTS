using System;
using HuginTS.Core.Domain.Entities;
using HuginTS.Core.Factories;

namespace HuginTS.Core.Domain
{
	public class PartitionMatrixOffsetCalculator
	{
		public static  PartitionMatrixSize CalculatePosition(PartitionGranularity granularity, DateTime timestamp)
		{
			switch (granularity)
			{
				case PartitionGranularity.Second:
					return new PartitionMatrixSize(timestamp.Hour *60 + timestamp.Minute, timestamp.Second);
				case PartitionGranularity.Minute:
					return new PartitionMatrixSize(timestamp.Hour, timestamp.Minute);
				case PartitionGranularity.Quarter:
					return new PartitionMatrixSize(timestamp.Hour, timestamp.Minute / 15);
				case PartitionGranularity.Hour:
					return new PartitionMatrixSize(timestamp.Hour, 1);
				default:
					throw new ArgumentException("Unknown granlarity");
			}

		}
	}
}