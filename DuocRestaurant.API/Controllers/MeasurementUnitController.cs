using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Services;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DuocRestaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementUnitController : ControllerBase
    {
        private IMeasurementUnitService measurementUnitService { get; set; }
        private RestaurantDatabaseSettings dbSettings { get; set; }

        public MeasurementUnitController(IMeasurementUnitService measurementUnitService, IOptions<RestaurantDatabaseSettings> databaseContext)
        {
            this.measurementUnitService = measurementUnitService;
            this.dbSettings = databaseContext.Value;
        }

        [HttpGet]
        [ActionName("GetAll")]
        [Route("[action]")]
        public IActionResult Get()
        {
            IActionResult result;

            try
            {
                result = Ok(this.measurementUnitService.Get(this.dbSettings).MapAll(true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}