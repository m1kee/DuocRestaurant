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
    public class TableController : ControllerBase
    {
        private ITableService tableService { get; set; }
        private RestaurantDatabaseSettings dbSettings { get; set; }

        public TableController(ITableService authService, IOptions<RestaurantDatabaseSettings> databaseContext)
        {
            this.tableService = authService;
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
                result = Ok(this.tableService.Get(this.dbSettings).MapAll(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpGet]
        [ActionName("GetById")]
        [Route("[action]")]
        public IActionResult Get([FromRoute(Name = "id")] int tableId)
        {
            IActionResult result;

            try
            {
                result = Ok(this.tableService.Get(this.dbSettings, tableId).Map(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpPost]
        public IActionResult Post([FromBody]Table table)
        {
            IActionResult result;

            try
            {
                var tables = this.tableService.Get(this.dbSettings);
                if (tables.Any(x => x.Number.Equals(table.Number)))
                    throw new Exception($"Ya existe una mesa con el número: { table.Number }");

                result = Ok(this.tableService.Add(this.dbSettings, table).Map(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute(Name = "id")] int tableId, [FromBody]Table table)
        {
            IActionResult result;

            try
            {
                var tables = this.tableService.Get(this.dbSettings);
                if (tables.Any(x => x.Id != tableId && x.Number.Equals(table.Number)))
                    throw new Exception($"Ya existe una mesa con el número: { table.Number }");

                result = Ok(this.tableService.Edit(this.dbSettings, tableId, table).Map(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute(Name = "id")] int tableId)
        {
            IActionResult result;

            try
            {
                result = Ok(this.tableService.Delete(this.dbSettings, tableId));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}