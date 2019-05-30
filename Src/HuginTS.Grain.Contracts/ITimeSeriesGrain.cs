using System.Threading.Tasks;
using HuginTS.Core.Models;
using Orleans;

namespace HuginTS.Grain.Contracts
{
    public interface ITimeSeriesGrain : IGrainWithGuidKey
    {
        Task Add(DatapointModel datapoint);
    }
}
