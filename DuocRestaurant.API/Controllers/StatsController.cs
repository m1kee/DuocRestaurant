using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Business.Services;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DuocRestaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private IPurchaseService purchaseService { get; set; }
        private IOrderService orderService { get; set; }
        public StatsController(
            IPurchaseService purchaseService,
            IOrderService orderService)
        {
            this.purchaseService = purchaseService;
            this.orderService = orderService;
        }

        [HttpGet]
        [ActionName("GetMonthlySells")]
        [Route("[action]")]
        public IActionResult GetMonthlySells()
        {
            IActionResult result;

            try
            {
                var response = new MonthSellsResponse();
                // get purchase
                var purchases = this.purchaseService.Get();
                if (purchases != null && purchases.Any())
                {
                    purchases = purchases.Where(x => x.StateId == (int)Enums.PurchaseState.Paid).ToList();
                    var categories = purchases.Select(x => x.CreationDate.Date).Distinct();

                    response.Categories = categories.Select(x => x.Date.ToString("dd-MM-yyyy")).ToList();
                    var serie = new ChartSerie()
                    {
                        Name = "Total diario",
                        Data = new List<object>()
                    };
                    foreach (var category in categories)
                    {
                        int value = 0;
                        var categoryPurchases = purchases.Where(x => x.CreationDate.Date.Equals(category.Date)).ToList();

                        foreach (var categoryPurchase in categoryPurchases)
                        {
                            value += categoryPurchase.Total;
                        }

                        serie.Data.Add(value);
                    }

                    response.Serie = serie;
                }

                result = Ok(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}
