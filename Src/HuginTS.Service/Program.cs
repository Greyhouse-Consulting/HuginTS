using System;
using System.Threading.Tasks;
using HuginTS.Core.Factories;
using HuginTS.Core.Infrastructure;
using HuginTS.Core.Infrastructure.Contracts;
using HuginTS.Core.Services;
using HuginTS.Core.Services.Contracts;
using HuginTS.Grain.Contracts;
using HuginTS.Grains.Sensor;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace HuginTS.Service
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                using(var siloHost = await StartSilo())
                {
                    CreateWebHostBuilder(args).Build().Run();

                    await siloHost.StopAsync();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return 1;
            }

            return 0;
        }

        private static async Task<ISiloHost> StartSilo()
        {
            // define the cluster configuration
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .AddMongoDBGrainStorageAsDefault(c => { c.ConnectionString = "mongodb://localhost:27017/hugints"; })
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .ConfigureServices(c =>
                {
                    c.TryAddTransient<IDataPointService, DataPointService>();
                    c.TryAddTransient<ITimeseriesRepository, TimeseriesRepository>();
                    c.TryAddTransient<ITimeseriesPartitionFactory, TimeseriesPartitionFactory>();
                    c.TryAddTransient<IMongoClient>(s => new MongoClient("mongodb://localhost:27017/hugints"));
                    c.TryAddTransient(s => ((IMongoClient)s.GetService(typeof(IMongoClient))).GetDatabase("hugints"));

                })
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(TimeSeriesGrain).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
