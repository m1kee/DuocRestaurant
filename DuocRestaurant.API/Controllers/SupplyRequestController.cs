using System;
using System.Linq;
using System.Text;
using Business.Services;
using Domain;
using DuocRestaurant.API.Model;
using DuocRestaurant.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace DuocRestaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplyRequestController : ControllerBase
    {
        private ISupplyRequestService supplyRequestService { get; set; }
        private IProductService productService { get; set; }
        private IProviderService providerService { get; set; }
        private IMailService mailService { get; set; }
        private IUserService userService { get; set; }
        private RestaurantDatabaseSettings dbSettings { get; set; }

        public SupplyRequestController(ISupplyRequestService supplyRequestService,
            IProductService productService,
            IProviderService providerService,
            IMailService mailService,
            IUserService userService,
            IOptions<RestaurantDatabaseSettings> databaseContext)
        {
            this.supplyRequestService = supplyRequestService;
            this.productService = productService;
            this.providerService = providerService;
            this.mailService = mailService;
            this.userService = userService;
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
                var supplyRequests = this.supplyRequestService.Get(this.dbSettings);
                var products = this.productService.Get(this.dbSettings).Where(x => x.Active).ToList();
                foreach (var supplyRequest in supplyRequests)
                {
                    if (supplyRequest.SupplyRequestDetails != null)
                    {
                        foreach (var supplyRequestDetail in supplyRequest.SupplyRequestDetails)
                        {
                            supplyRequestDetail.Product = products.FirstOrDefault(x => x.Id == supplyRequestDetail.ProductId);
                        }
                    }
                }

                result = Ok(supplyRequests.MapAll(this.dbSettings, true));
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
        public IActionResult Get([FromRoute(Name = "id")] int supplyRequestId)
        {
            IActionResult result;

            try
            {
                var supplyRequest = this.supplyRequestService.Get(this.dbSettings, supplyRequestId);

                if (supplyRequest == null)
                    throw new Exception($"No se encontró la orden de compra con el Id: {supplyRequestId}");

                if (supplyRequest.SupplyRequestDetails != null)
                {
                    var products = this.productService.Get(this.dbSettings).Where(x => x.Active).ToList();
                    foreach (var supplyRequestDetail in supplyRequest.SupplyRequestDetails)
                    {
                        supplyRequestDetail.Product = products.FirstOrDefault(x => x.Id == supplyRequestDetail.ProductId);
                    }
                }

                result = Ok(supplyRequest.Map(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpGet]
        [ActionName("GetById")]
        [Route("[action]/{id}")]
        public IActionResult Get([FromRoute(Name = "id")] string supplyRequestCode)
        {
            IActionResult result;

            try
            {
                var supplyRequest = this.supplyRequestService.GetByCode(this.dbSettings, supplyRequestCode);
                if (supplyRequest == null)
                    throw new Exception($"No se encontró una orden de compra activa con el código: {supplyRequestCode}");

                result = Ok(supplyRequest.Map(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpPost]
        public IActionResult Post([FromBody] SupplyRequest supplyRequest)
        {
            IActionResult result;

            try
            {
                // always create a new supply request as created
                supplyRequest.StateId = (int)Enums.SupplyRequestState.Created;

                var created = this.supplyRequestService.Add(this.dbSettings, supplyRequest);

                if (created.SupplyRequestDetails != null && created.SupplyRequestDetails.Any())
                {
                    var products = this.productService.Get(this.dbSettings).Where(x => x.Active).ToList();
                    foreach (var supplyRequestDetail in created.SupplyRequestDetails)
                    {
                        supplyRequestDetail.Product = products.FirstOrDefault(x => x.Id == supplyRequestDetail.ProductId);
                    }
                }

                created.State = this.supplyRequestService.GetState(this.dbSettings, created.StateId);
                created.Provider = this.providerService.Get(this.dbSettings, created.ProviderId);

                result = Ok(created.Map(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute(Name = "id")] int supplyRequestId, [FromBody] SupplyRequest supplyRequest)
        {
            IActionResult result;

            try
            {
                bool sendMail = false;
                if (supplyRequest.StateId == (int)Enums.SupplyRequestState.Sended)
                {
                    var bdSupplyRequest = this.supplyRequestService.Get(this.dbSettings, supplyRequestId);
                    if (bdSupplyRequest.StateId == (int)Enums.SupplyRequestState.Created)
                        sendMail = true;
                }

                var edited = this.supplyRequestService.Edit(this.dbSettings, supplyRequestId, supplyRequest);

                edited.SupplyRequestDetails = this.supplyRequestService.Get(this.dbSettings, edited).ToList();

                if (edited.SupplyRequestDetails != null && edited.SupplyRequestDetails.Any())
                {
                    var products = this.productService.Get(this.dbSettings).Where(x => x.Active).ToList();
                    foreach (var supplyRequestDetail in edited.SupplyRequestDetails)
                    {
                        supplyRequestDetail.Product = products.FirstOrDefault(x => x.Id == supplyRequestDetail.ProductId);
                    }
                }

                edited.State = this.supplyRequestService.GetState(this.dbSettings, edited.StateId);
                edited.Provider = this.providerService.Get(this.dbSettings, edited.ProviderId);

                if (sendMail)
                {
                    // insert here the logic of the email 
                    StringBuilder sb = new StringBuilder();
                    if (edited.SupplyRequestDetails != null && edited.SupplyRequestDetails.Any())
                    {
                        sb.AppendLine($"<ul>");
                        foreach (var detail in edited.SupplyRequestDetails)
                        {
                            sb.AppendLine($"<li><b>{detail.Count} x</b> {detail.Product.Name}</li>");
                        }
                        sb.AppendLine($"</ul>");
                    }


                    this.mailService.SendMail(edited.Provider.Email,
                        $"Orden de compra: {string.Format(new FormatterHelper(), "{0:G}", edited.Code)}",
                        $"Estimado: <b>{edited.Provider.Name}</b> <br/><br/> Hemos emitido la siguiente orden de compra con código: <b>{string.Format(new FormatterHelper(), "{0:G}", edited.Code)}</b>. La cual contiene el siguiente detalle: <br/><br/> {sb} <br/><br/> Se despide atentamente <b>Restaurant Siglo XXI</b>.");
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
        public IActionResult Delete([FromRoute(Name = "id")] int supplyRequestId)
        {
            IActionResult result;

            try
            {
                result = Ok(this.supplyRequestService.Delete(this.dbSettings, supplyRequestId));
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
                var supplyRequests = this.supplyRequestService.Get(this.dbSettings);

                if (filters.ContainsKey("StateId"))
                {
                    int stateId = Convert.ToInt32(filters.GetValue("StateId").ToString());

                    supplyRequests = supplyRequests.Where(x => x.StateId == stateId).ToList();
                }

                if (filters.ContainsKey("Code"))
                {
                    string code = filters.GetValue("Code").ToString();

                    supplyRequests = supplyRequests.Where(x => x.Code.Contains(code)).ToList();
                }

                result = Ok(supplyRequests.MapAll(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [ActionName("Finalize")]
        [Route("[action]")]
        public IActionResult Finalize([FromBody] FinalizeSupplyRequest request)
        {
            IActionResult result;

            try
            {
                User user = this.userService.Get(this.dbSettings, request.UserId);
                var supplyRequest = this.supplyRequestService.Get(this.dbSettings, request.SupplyRequestId);
                supplyRequest.StateId = (int)request.SupplyRequestState;

                var edited = this.supplyRequestService.Edit(this.dbSettings, request.SupplyRequestId, supplyRequest);

                StringBuilder finalizeMessage = new StringBuilder();
                finalizeMessage.AppendLine($"Estimado: <b>{edited.Provider.Name}</b> <br/><br/>");
                if (request.SupplyRequestState == Enums.SupplyRequestState.Confirmed)
                {
                    finalizeMessage.AppendLine($"El usuario <b>{user.Name} {user.LastName}</b> ha confirmado la recepción de la orden de compra con código: <b>{string.Format(new FormatterHelper(), "{0:G}", edited.Code)}</b>");
                }
                else if (request.SupplyRequestState == Enums.SupplyRequestState.Rejected)
                {
                    finalizeMessage.AppendLine($"El usuario <b>{user.Name} {user.LastName}</b> ha rechazado la recepción de la orden de compra con código: <b>{string.Format(new FormatterHelper(), "{0:G}", edited.Code)}</b> <br/><br/>");
                    finalizeMessage.AppendLine($"Razón: {request.Reason}");
                }
                finalizeMessage.AppendLine($"<br/><br/> Se despide atentamente <b>Restaurant Siglo XXI</b>.");

                this.mailService.SendMail(edited.Provider.Email,
                        $"Orden {(request.SupplyRequestState == Enums.SupplyRequestState.Confirmed ? "Recepcionada" : "Rechazada")}: {string.Format(new FormatterHelper(), "{0:G}", edited.Code)}",
                        $"{finalizeMessage}");

                result = Ok(edited.Map(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}
