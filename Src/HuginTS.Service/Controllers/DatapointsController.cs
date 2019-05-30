using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HuginTS.Core;
using HuginTS.Core.Models;
using HuginTS.Core.Services.Contracts;
using HuginTS.Grain.Contracts;
using HuginTS.Service.Models;
using HuginTS.Service.Validators;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace HuginTS.Service.Controllers
{
    [Route("data-points")]
    [ApiController]
    public class DataPointsController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;
        private readonly IDataPointService _dataPointService;
        private readonly DataPointValidator _dataPointValidator;

        public DataPointsController(
            IClusterClient clusterClient,
            IDataPointService dataPointService,
            DataPointValidator dataPointValidator)
        {
            _clusterClient = clusterClient;
            _dataPointService = dataPointService;
            _dataPointValidator = dataPointValidator;
        }

        // POST api/data-points
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Datapoint dataPoint)
        {
            var result = _dataPointValidator.Validate(dataPoint);

            if(!result.IsValid)
                return new BadRequestResult();

            try
            {
                var timeSeriesId = IdGenerator.CreateGuidId(dataPoint.Name);

                await _clusterClient
                        .GetGrain<ITimeSeriesGrain>(timeSeriesId)
                        .Add(new DatapointModel(dataPoint.Name, dataPoint.Timestamp, dataPoint.Value));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get(string name, DateTime from, DateTime to)
        {
            var id = IdGenerator.CreateObjectId(name);
            return Ok(await _dataPointService.GetByPeriodAsync(name, from, to));
        }
    }


}
