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
    public class InventoryController : ControllerBase
    {
        private IInventoryService _inventoryService { get; set; }
        private RestaurantDatabaseSettings _dbSettings { get; set; }

        public InventoryController(IInventoryService inventoryService, IOptions<RestaurantDatabaseSettings> databaseContext)
        {
            this._inventoryService = inventoryService;
            this._dbSettings = databaseContext.Value;
        }

        /// <summary>
        /// This method returns all inventorys
        /// </summary>
        /// <returns>List<inventory></returns>
        [HttpGet]
        [ActionName("GetAll")]
        [Route("[action]")]
        public IActionResult Get()
        {
            IActionResult result;

            try
            {
                result = Ok(this._inventoryService.Get(this._dbSettings).MapAll(true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// This method returns a specific inventory
        /// </summary>
        /// <param name="inventoryId"></param>
        /// <returns>inventory</returns>
        [HttpGet]
        [ActionName("GetById")]
        [Route("[action]")]
        public IActionResult Get([FromRoute(Name = "id")] int inventoryId)
        {
            IActionResult result;

            try
            {
                result = Ok(this._inventoryService.Get(this._dbSettings, inventoryId).Map(true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// This method inserts a new inventory
        /// </summary>
        /// <param name="inventory"></param>
        /// <returns>Inserted inventory</returns>
        [HttpPost]
        public IActionResult Post([FromBody]Inventory inventory)
        {
            IActionResult result;

            try
            {
                result = Ok(this._inventoryService.Add(this._dbSettings, inventory));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// This method update a inventory by it's Id
        /// </summary>
        /// <param name="inventoryId"></param>
        /// <param name="inventory"></param>
        /// <returns>Updated inventory</returns>
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute(Name = "id")] int inventoryId, [FromBody]Inventory inventory)
        {
            IActionResult result;

            try
            {
                // TODO: validate things
                result = Ok(this._inventoryService.Edit(this._dbSettings, inventoryId, inventory));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// this method deletes a inventory by it's Id
        /// </summary>
        /// <param name="inventoryId"></param>
        /// <returns>Deleted inventory</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute(Name = "id")] int inventoryId)
        {
            IActionResult result;

            try
            {
                // TODO: validate things
                result = Ok(this._inventoryService.Delete(this._dbSettings, inventoryId));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}