using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Services;
using Domain;
using DuocRestaurant.API.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace DuocRestaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderService orderService { get; set; }
        private IProductService productService { get; set; }
        private IPurchaseService purchaseService { get; set; }
        private IRecipeService recipeService { get; set; }
        private IUserService userService { get; set; }
        private RestaurantDatabaseSettings dbSettings { get; set; }
        private IHubContext<OrdersHub, IOrderClient> ordersHub { get; set; }
        private IHubContext<NotificationsHub, INotificationClient> notificationsHub { get; set; }

        public OrderController(IOrderService orderService,
            IProductService productService,
            IPurchaseService purchaseService,
            IRecipeService recipeService,
            IProviderService providerService,
            IUserService userService,
            IOptions<RestaurantDatabaseSettings> databaseContext,
            IHubContext<OrdersHub, IOrderClient> ordersHub,
            IHubContext<NotificationsHub, INotificationClient> notificationsHub
        )
        {
            this.orderService = orderService;
            this.productService = productService;
            this.purchaseService = purchaseService;
            this.recipeService = recipeService;
            this.userService = userService;
            this.dbSettings = databaseContext.Value;
            this.ordersHub = ordersHub;
            this.notificationsHub = notificationsHub;
        }

        [HttpGet]
        [ActionName("GetAll")]
        [Route("[action]")]
        public IActionResult Get()
        {
            IActionResult result;

            try
            {
                var orders = this.orderService.Get();
                var products = this.productService.Get(this.dbSettings).Where(x => x.Active).ToList();
                var recipes = this.recipeService.Get(this.dbSettings).Where(x => x.Active).ToList();
                foreach (var order in orders)
                {
                    if (order.OrderDetails != null)
                    {
                        foreach (var orderDetail in order.OrderDetails)
                        {
                            if (orderDetail.ProductId != null)
                                orderDetail.Product = products.FirstOrDefault(x => x.Id == orderDetail.ProductId);
                            
                            if (orderDetail.RecipeId != null)
                                orderDetail.Recipe = recipes.FirstOrDefault(x => x.Id == orderDetail.RecipeId);
                        }
                    }

                    if (order.PurchaseId != null)
                        order.Purchase = this.purchaseService.Get((int)order.PurchaseId);
                }

                result = Ok(orders.MapAll(this.dbSettings, true));
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
        public IActionResult Get([FromRoute(Name = "id")] int orderId)
        {
            IActionResult result;

            try
            {
                var order = this.orderService.Get(orderId);

                if (order == null)
                    throw new Exception($"No se encontró la orden con el Id: {orderId}");

                if (order.OrderDetails != null)
                {
                    var products = this.productService.Get(this.dbSettings).Where(x => x.Active).ToList();
                    var recipes = this.recipeService.Get(this.dbSettings).Where(x => x.Active).ToList();
                    foreach (var orderDetail in order.OrderDetails)
                    {
                        if (orderDetail.ProductId != null)
                            orderDetail.Product = products.FirstOrDefault(x => x.Id == orderDetail.ProductId);

                        if (orderDetail.RecipeId != null)
                            orderDetail.Recipe = recipes.FirstOrDefault(x => x.Id == orderDetail.RecipeId);
                    }
                }

                if (order.PurchaseId != null)
                    order.Purchase = this.purchaseService.Get((int)order.PurchaseId);

                result = Ok(order.Map(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Order order)
        {
            IActionResult result;

            try
            {
                // always create a new supply request as pending
                order.StateId = (int)Enums.OrderState.Pending;

                var created = this.orderService.Add(order);

                if (created.OrderDetails != null && created.OrderDetails.Any())
                {
                    var products = this.productService.Get(this.dbSettings).Where(x => x.Active).ToList();
                    var recipes = this.recipeService.Get(this.dbSettings).Where(x => x.Active).ToList();
                    foreach (var orderDetail in created.OrderDetails)
                    {
                        if (orderDetail.ProductId != null)
                            orderDetail.Product = products.FirstOrDefault(x => x.Id == orderDetail.ProductId);

                        if (orderDetail.RecipeId != null)
                            orderDetail.Recipe = recipes.FirstOrDefault(x => x.Id == orderDetail.RecipeId);
                    }
                }

                if (created.PurchaseId != null)
                    created.Purchase = this.purchaseService.Get((int)created.PurchaseId);

                // call reload orders in all connected clients
                this.ordersHub.Clients.All.ReloadOrders();

                result = Ok(created.Map(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute(Name = "id")] int orderId, [FromBody] Order order)
        {
            IActionResult result;

            try
            {
                bool sendReadyNotification = false;
                if (order.StateId == (int)Enums.OrderState.Ready)
                {
                    var bdOrder = this.orderService.Get(orderId);
                    if (bdOrder.StateId == (int)Enums.OrderState.InPreparation)
                        sendReadyNotification = true;
                }

                var edited = this.orderService.Edit(orderId, order);

                edited.OrderDetails = this.orderService.Get(edited).ToList();

                if (edited.OrderDetails != null && edited.OrderDetails.Any())
                {
                    var products = this.productService.Get(this.dbSettings).Where(x => x.Active).ToList();
                    var recipes = this.recipeService.Get(this.dbSettings).Where(x => x.Active).ToList();
                    foreach (var orderDetail in edited.OrderDetails)
                    {
                        if (orderDetail.ProductId != null)
                            orderDetail.Product = products.FirstOrDefault(x => x.Id == orderDetail.ProductId);

                        if (orderDetail.RecipeId != null)
                            orderDetail.Recipe = recipes.FirstOrDefault(x => x.Id == orderDetail.RecipeId);
                    }
                }

                if (edited.PurchaseId != null)
                    edited.Purchase = this.purchaseService.Get((int)edited.PurchaseId);

                if (sendReadyNotification)
                {
                    string notification = $"El pedido para la mesa { edited.Table.Number } está listo.";
                    var waiters = this.userService.Get(this.dbSettings).Where(x => x.RoleId == (int)Enums.Profile.Waiter).ToList();
                    if (waiters.Any())
                    {
                        foreach (var waiter in waiters)
                        {
                            this.notificationsHub.Clients.All.Notify(waiter.Id, notification);
                        }
                    }
                }

                result = Ok(edited.Map(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute(Name = "id")] int orderId)
        {
            IActionResult result;

            try
            {
                result = Ok(this.orderService.Delete(orderId));
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
                var orders = this.orderService.Get();

                if (filters.ContainsKey("StateId"))
                {
                    int stateId = Convert.ToInt32(filters.GetValue("StateId").ToString());

                    orders = orders.Where(x => x.StateId == stateId).ToList();
                }

                if (filters.ContainsKey("UserId"))
                {
                    int userId = Convert.ToInt32(filters.GetValue("UserId").ToString());

                    orders = orders.Where(x => x.UserId == userId).ToList();
                }

                if (filters.ContainsKey("States"))
                {
                    List<int> states = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(filters.GetValue("States").ToString());

                    orders = orders.Where(x => states.Any(y => y == x.StateId)).ToList();
                }

                if (filters.ContainsKey("PuchaseId"))
                {
                    int purchaseId = Convert.ToInt32(filters.GetValue("PuchaseId").ToString());

                    orders = orders.Where(x => x.PurchaseId == purchaseId).ToList();
                }

                if (orders.Any())
                {
                    var products = this.productService.Get(this.dbSettings).Where(x => x.Active).ToList();
                    var recipes = this.recipeService.Get(this.dbSettings).Where(x => x.Active).ToList();

                    foreach (var order in orders)
                    {
                        if (order.OrderDetails != null)
                        {
                            foreach (var orderDetail in order.OrderDetails)
                            {
                                if (orderDetail.ProductId != null)
                                    orderDetail.Product = products.FirstOrDefault(x => x.Id == orderDetail.ProductId);

                                if (orderDetail.RecipeId != null)
                                    orderDetail.Recipe = recipes.FirstOrDefault(x => x.Id == orderDetail.RecipeId);
                            }
                        }

                        if (order.PurchaseId != null)
                            order.Purchase = this.purchaseService.Get((int)order.PurchaseId);
                    }
                }

                result = Ok(orders.MapAll(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}
