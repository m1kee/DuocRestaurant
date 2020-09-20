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
    public class ProductController : ControllerBase
    {
        private IProductService productService { get; set; }
        private RestaurantDatabaseSettings dbSettings { get; set; }

        public ProductController(IProductService productService, IOptions<RestaurantDatabaseSettings> databaseContext)
        {
            this.productService = productService;
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
                result = Ok(this.productService.Get(this.dbSettings).MapAll(true));
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
        public IActionResult Get([FromRoute(Name = "id")] int productId)
        {
            IActionResult result;

            try
            {
                result = Ok(this.productService.Get(this.dbSettings, productId).Map(true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpPost]
        public IActionResult Post([FromBody]Product product)
        {
            IActionResult result;

            try
            {
                var products = this.productService.Get(this.dbSettings);
                if (products.Any(x => x.Name.Equals(product.Name, StringComparison.InvariantCultureIgnoreCase)))
                    throw new Exception($"Ya existe un producto con el nombre: { product.Name }");

                result = Ok(this.productService.Add(this.dbSettings, product).Map(true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute(Name = "id")] int productId, [FromBody]Product product)
        {
            IActionResult result;

            try
            {
                var products = this.productService.Get(this.dbSettings);
                if (products.Any(x => x.Id != productId && x.Name.Equals(product.Name, StringComparison.InvariantCultureIgnoreCase)))
                    throw new Exception($"Ya existe un producto con el nombre: { product.Name }");

                result = Ok(this.productService.Edit(this.dbSettings, productId, product).Map(true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute(Name = "id")] int productId)
        {
            IActionResult result;

            try
            {
                result = Ok(this.productService.Delete(this.dbSettings, productId));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}