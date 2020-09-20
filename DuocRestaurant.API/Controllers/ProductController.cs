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
        private IMeasurementUnitService measurementUnitService { get; set; }
        private IProductTypeService productTypeService { get; set; }
        private IProviderService providerService { get; set; }
        private RestaurantDatabaseSettings dbSettings { get; set; }

        public ProductController(IProductService productService, 
            IMeasurementUnitService measurementUnitService,
            IProductTypeService productTypeService,
            IProviderService providerService,
            IOptions<RestaurantDatabaseSettings> databaseContext)
        {
            this.productService = productService;
            this.measurementUnitService = measurementUnitService;
            this.productTypeService = productTypeService;
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
                result = Ok(this.productService.Get(this.dbSettings).MapAll(this.dbSettings, true));
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
                result = Ok(this.productService.Get(this.dbSettings, productId).Map(this.dbSettings, true));
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

                var created = this.productService.Add(this.dbSettings, product);

                created.MeasurementUnit = this.measurementUnitService.Get(this.dbSettings, created.MeasurementUnitId);
                created.Provider = this.providerService.Get(this.dbSettings, created.ProviderId);
                created.ProductType = this.productTypeService.Get(this.dbSettings, created.ProductTypeId);

                result = Ok(created.Map(this.dbSettings, true));
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

                var edited = this.productService.Edit(this.dbSettings, productId, product);

                edited.MeasurementUnit = this.measurementUnitService.Get(this.dbSettings, edited.MeasurementUnitId);
                edited.Provider = this.providerService.Get(this.dbSettings, edited.ProviderId);
                edited.ProductType = this.productTypeService.Get(this.dbSettings, edited.ProductTypeId);

                result = Ok(edited.Map(this.dbSettings, true));
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