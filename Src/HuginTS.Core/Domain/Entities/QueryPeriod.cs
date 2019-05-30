using System;

namespace HuginTS.Core.Domain.Entities
{
    public class QueryPeriod
    {
        public string Name { get; }
        public DateTime Start { get; }
        public DateTime End { get; }

        public QueryPeriod(string name, DateTime start, DateTime end)
        {
            Name = name;
            Start = start;
            End = end;
        }
    }
}