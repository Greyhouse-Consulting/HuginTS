using System;
using HuginTS.Core.Infrastructure;
using MongoDB.Bson;

namespace HuginTS.Core.Domain.Entities
{
	public class TimeSerie  : IIdentifier
	{
		public ObjectId Id { get; set; } 
		public string Name { get; set; } 
		public string Description { get; set; }

		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }
		public PartitionGranularity Granularity { get; set; }


	}
}