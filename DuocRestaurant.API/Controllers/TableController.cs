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
    [ApiController]
    [Route("[controller]")]
    public class TableController : ControllerBase
    {
        private ITableService _tableService { get; set; }
        private RestaurantDatabaseSettings _dbSettings { get; set; }

        public TableController(ITableService authService, IOptions<RestaurantDatabaseSettings> databaseContext)
        {
            this._tableService = authService;
            this._dbSettings = databaseContext.Value;
        }

        /// <summary>
        /// This method returns all tables
        /// </summary>
        /// <returns>List<Table></returns>
        [HttpGet]
        [ActionName("GetAll")]
        public IActionResult Get()
        {
            IActionResult result;

            try
            {
                result = Ok(this._tableService.Get(this._dbSettings).MapAll(true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// This method returns a specific table
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns>Table</returns>
        [HttpGet]
        [ActionName("GetById")]
        public IActionResult Get([FromRoute(Name = "id")] int tableId)
        {
            IActionResult result;

            try
            {
                result = Ok(this._tableService.Get(this._dbSettings, tableId).Map(true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// This method inserts a new table
        /// </summary>
        /// <param name="table"></param>
        /// <returns>Inserted Table</returns>
        [HttpPost]
        public IActionResult Post([FromBody]Table table)
        {
            IActionResult result;

            try
            {
                result = Ok(this._tableService.Add(this._dbSettings, table));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// This method update a table by it's Id
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="table"></param>
        /// <returns>Updated Table</returns>
        [HttpPut]
        public IActionResult Put([FromRoute(Name = "id")] int tableId, [FromBody]Table table)
        {
            IActionResult result;

            try
            {
                // TODO: validate things
                result = Ok(this._tableService.Edit(this._dbSettings, tableId, table));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// this method deletes a table by it's Id
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns>Deleted Table</returns>
        [HttpDelete]
        public IActionResult Delete([FromRoute(Name = "id")] int tableId)
        {
            IActionResult result;

            try
            {
                // TODO: validate things
                result = Ok(this._tableService.Delete(this._dbSettings, tableId));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}