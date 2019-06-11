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

        [Fact]
        public async void Should_Store_Two_DataPoints_On_Different_Partitions()
        {
            var runner = MongoDbRunner.Start();
            var client = new MongoClient(runner.ConnectionString);

            // Arrange
            var dataPointService = new DataPointService(new TimeseriesPartitionFactory(client.GetDatabase("test")));

            var timeStamp1 = new DateTime(2019, 1, 1, 1, 0, 0);
            var timeStamp2 = new DateTime(2019, 1, 2, 1, 0, 0);

            await dataPointService.RegisterAsync("series1", timeStamp1, 20);
            await dataPointService.RegisterAsync("series1", timeStamp2, 30);

            // Act

            var dataPoints = await dataPointService.GetByPeriodAsync("series1", new DateTime(2019, 1, 1, 0, 0, 0),
                new DateTime(2019, 1, 20, 3, 0, 0));

            // Assert
            dataPoints.Count.ShouldBe(2);
            dataPoints[0].Timestamp.ShouldBe(timeStamp1);
            dataPoints[0].Value.ShouldBe(20);
            dataPoints[1].Timestamp.ShouldBe(timeStamp2);
            dataPoints[1].Value.ShouldBe(30);

        }
    }
}
