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
    public class ProviderController : ControllerBase
    {
        private IProviderService providerService { get; set; }
        private RestaurantDatabaseSettings dbSettings { get; set; }

        public ProviderController(IProviderService providerService, IOptions<RestaurantDatabaseSettings> databaseContext)
        {
            this.providerService = providerService;
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
                result = Ok(this.providerService.Get(this.dbSettings).MapAll(true));
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
        public IActionResult Get([FromRoute(Name = "id")] int providerId)
        {
            IActionResult result;

            try
            {
                result = Ok(this.providerService.Get(this.dbSettings, providerId).Map(true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpPost]
        public IActionResult Post([FromBody]Provider provider)
        {
            IActionResult result;

            try
            {
                var providers = this.providerService.Get(this.dbSettings);
                if (providers.Any(x => x.Email.Equals(provider.Email, StringComparison.InvariantCultureIgnoreCase)))
                    throw new Exception($"Ya existe un proveedor con el correo: { provider.Email }");

                result = Ok(this.providerService.Add(this.dbSettings, provider).Map(true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute(Name = "id")] int providerId, [FromBody]Provider provider)
        {
            IActionResult result;

            try
            {
                var providers = this.providerService.Get(this.dbSettings);
                if (providers.Any(x => x.Id != providerId && x.Email.Equals(provider.Email, StringComparison.InvariantCultureIgnoreCase)))
                    throw new Exception($"Ya existe un proveedor con el correo: { provider.Email }");

                result = Ok(this.providerService.Edit(this.dbSettings, providerId, provider).Map(true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute(Name = "id")] int providerId)
        {
            IActionResult result;

            try
            {
                result = Ok(this.providerService.Delete(this.dbSettings, providerId));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}