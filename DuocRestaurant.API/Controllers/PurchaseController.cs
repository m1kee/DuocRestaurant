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
        private IOrderService orderService { get; set; }
        private IFlowService flowService { get; set; }
        private IUserService userService { get; set; }
        private ITableService tableService { get; set; }
        private RestaurantDatabaseSettings dbSettings { get; set; }

        public PurchaseController(IPurchaseService purchaseService,
            IOrderService orderService,
            IFlowService flowService,
            IUserService userService,
            ITableService tableService,
            IOptions<RestaurantDatabaseSettings> databaseContext)
        {
            this.purchaseService = purchaseService;
            this.userService = userService;
            this.flowService = flowService;
            this.tableService = tableService;
            this.dbSettings = databaseContext.Value;
            this.orderService = orderService;
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

                if (created != null && created.Id > 0)
                {
                    User user = null;
                    Table table = null;
                    if (purchase.Orders != null && purchase.Orders.Any())
                    {
                        foreach (var order in purchase.Orders)
                        {
                            if (order == purchase.Orders.First())
                            {
                                user = order.User ?? this.userService.Get(this.dbSettings, order.UserId);
                                table = order.Table ?? this.tableService.Get(this.dbSettings, order.TableId);
                            }

                            order.PurchaseId = purchase.Id;
                            this.orderService.Edit(order.Id, order);
                        }
                    }

                    // generate flow payment
                    var response = this.flowService.CreateEmailPayment(user.Email, created.Total, $"Pago cuenta mesa: {table.Number} - {user.Name} {user.LastName}", purchase.Id);
                    if (response != null)
                    {
                        created.URL = response.URL;
                        created.Token = response.Token;
                        created.FlowOrder = response.FlowOrder;

                        created = this.purchaseService.Edit(created.Id, created);
                    }
                }

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

                if (filters.ContainsKey("UserId"))
                {
                    int userId = Convert.ToInt32(filters.GetValue("UserId").ToString());
                    var orders = this.orderService.Get();
                    if (orders != null && orders.Any())
                    {
                        var userOrders = orders.Where(x => x.UserId == userId).ToList();
                        purchases = purchases.Where(x => orders.Any(y => y.PurchaseId == x.Id)).ToList();
                    }
                }

                if (filters.ContainsKey("Month"))
                {
                    int month = Convert.ToInt32(filters.GetValue("Month").ToString());
                    DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, (month + 1), 1, 0, 0, 0);
                    DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddTicks(-1);

                    purchases = purchases.Where(x => x.CreationDate >= firstDayOfMonth && x.CreationDate <= lastDayOfMonth).ToList();
                }

                result = Ok(purchases.OrderByDescending(x => x.CreationDate).MapAll(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpGet]
        [ActionName("ValidatePayment")]
        [Route("[action]/{id:int}")]
        public IActionResult ValidatePayment([FromRoute(Name = "id")] int purchaseId)
        {
            IActionResult result;

            try
            {
                // get purchase
                var purchase = this.purchaseService.Get(purchaseId);
                if (purchase == null)
                    throw new Exception($"No existe una compra con ID: {purchaseId}");

                // get status of the payment
                var paymentStatus = this.flowService.GetStatus(purchaseId);
                if (paymentStatus != null)
                {
                    switch (paymentStatus.Status)
                    {
                        case (int)Enums.PurchaseState.PendingPayment:
                            purchase.StateId = (int)Enums.PurchaseState.PendingPayment;
                            break;
                        case (int)Enums.PurchaseState.Paid:
                            purchase.StateId = (int)Enums.PurchaseState.Paid;
                            break;
                        case (int)Enums.PurchaseState.Rejected:
                            purchase.StateId = (int)Enums.PurchaseState.Rejected;
                            break;
                        case (int)Enums.PurchaseState.Canceled:
                            purchase.StateId = (int)Enums.PurchaseState.Canceled;
                            break;
                    }

                    // edit the payment status
                    this.purchaseService.Edit(purchaseId, purchase);
                }

                // return the edited purchase
                result = Ok(purchase.Map(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}
