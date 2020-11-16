using System;
using System.Linq;
using Business.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace DuocRestaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private IPurchaseService purchaseService{ get; set; }
        private IUserService userService { get; set; }
        private ITableService tableService { get; set; }
        private RestaurantDatabaseSettings dbSettings { get; set; }

        public PurchaseController(IPurchaseService purchaseService,
            IUserService userService,
            ITableService tableService,
            IOptions<RestaurantDatabaseSettings> databaseContext)
        {
            this.purchaseService = purchaseService;
            this.userService = userService;
            this.tableService = tableService;
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
                result = Ok(this.purchaseService.Get().MapAll(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpGet]
        [ActionName("GetById")]
        [Route("[action]/{id:int}")]
        public IActionResult Get([FromRoute(Name = "id")] int purchaseId)
        {
            IActionResult result;

            try
            {
                result = Ok(this.purchaseService.Get(purchaseId).Map(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Purchase purchase)
        {
            IActionResult result;

            try
            {
                var created = this.purchaseService.Add(purchase);

                result = Ok(created.Map(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute(Name = "id")] int purchaseId, [FromBody] Purchase purchase)
        {
            IActionResult result;

            try
            {
                var edited = this.purchaseService.Edit(purchaseId, purchase);

                result = Ok(edited.Map(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute(Name = "id")] int purchaseId)
        {
            IActionResult result;

            try
            {
                result = Ok(this.purchaseService.Delete(purchaseId));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [ActionName("FilterBy")]
        [Route("[action]")]
        public IActionResult FilterBy([FromBody] JObject filters)
        {
            IActionResult result;

            try
            {
                var purchases = this.purchaseService.Get();

                if (filters.ContainsKey("StateId"))
                {
                    int stateId = Convert.ToInt32(filters.GetValue("StateId").ToString());

                    purchases = purchases.Where(x => x.StateId == stateId).ToList();
                }

                result = Ok(purchases.MapAll(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}
