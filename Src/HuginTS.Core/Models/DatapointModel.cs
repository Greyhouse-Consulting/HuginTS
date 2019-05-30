using System;

namespace HuginTS.Core.Models
{
    public class DatapointModel
    {
        public string Name { get; }
        public DateTime Timestamp { get; }
        public double Value { get; }

        public DatapointModel(string name, DateTime timestamp, double value)
        {
            Name = name;
            Timestamp = timestamp;
            Value = value;
        }
    }
}