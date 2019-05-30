using HuginTS.Core.Infrastructure.Contracts;

namespace HuginTS.Core.Factories
{
	public interface ITimeseriesPartitionFactory
	{
		ITimeseriesPartitionRepository Create(string name);
	}
}