using System;

namespace HuginTS.Service.Models
{
    public class Datapoint
    {
        public DateTime Timestamp { get; set; }

        public string Name	{ get; set; }

        public double Value { get; set; }
    }
}