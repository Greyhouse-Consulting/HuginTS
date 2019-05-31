using System;
using System.Linq;
using HuginTS.Core.Factories;
using HuginTS.Core.Services;
using Mongo2Go;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace HuginTS.Integration.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public async void Should_Store_One_Data_Point()
        {
            var runner = MongoDbRunner.Start();
            var client = new MongoClient(runner.ConnectionString);

            // Arrange
            var dataPointService = new DataPointService(new TimeseriesPartitionFactory(client.GetDatabase("test")));

            var timeStamp = new DateTime(2019, 1, 1, 1, 0, 0);

            // Act
            await dataPointService.RegisterAsync("series1", timeStamp, 20);

            var dataPoints = await dataPointService.GetByPeriodAsync("series1", new DateTime(2019, 1, 1, 0, 0, 0),
                new DateTime(2019, 1, 1, 3, 0, 0));

            // Assert
            dataPoints.Count.ShouldBe(1);
            dataPoints.First().Timestamp.ShouldBe(timeStamp);
            dataPoints.First().Value.ShouldBe(20);
        }
    }
}
