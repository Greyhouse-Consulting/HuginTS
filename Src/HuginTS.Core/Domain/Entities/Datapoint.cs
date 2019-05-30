using System;

namespace HuginTS.Core.Domain.Entities
{
	public class Datapoint
	{
        public Datapoint(DateTime timestamp, double value)
        {
            Timestamp = timestamp;
            Value = value;
        }

        public DateTime Timestamp { get;  }

		public double Value { get; }
	}
}