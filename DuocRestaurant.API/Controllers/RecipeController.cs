using Business.Services;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;

namespace DuocRestaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private IRecipeService recipeService { get; set; }
        private IProductService productService { get; set; }
        private RestaurantDatabaseSettings dbSettings { get; set; }

        public RecipeController(IRecipeService recipeService,
            IProductService productService,
            IOptions<RestaurantDatabaseSettings> databaseContext)
        {
            this.recipeService = recipeService;
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
                var recipes = this.recipeService.Get(this.dbSettings);
                var products = this.productService.Get(this.dbSettings).Where(x => x.Active).ToList();
                foreach (var recipe in recipes)
                {
                    if (recipe.RecipeDetails != null)
                    {
                        foreach (var recipeDetail in recipe.RecipeDetails)
                        {
                            recipeDetail.Product = products.FirstOrDefault(x => x.Id == recipeDetail.ProductId);
                        }
                    }
                }

                result = Ok(recipes.MapAll(this.dbSettings, true));
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
        public IActionResult Get([FromRoute(Name = "id")] int recipeId)
        {
            IActionResult result;

            try
            {
                var recipe = this.recipeService.Get(this.dbSettings, recipeId);

                if (recipe.RecipeDetails != null)
                {
                    var products = this.productService.Get(this.dbSettings).Where(x => x.Active).ToList();
                    foreach (var recipeDetail in recipe.RecipeDetails)
                    {
                        recipeDetail.Product = products.FirstOrDefault(x => x.Id == recipeDetail.ProductId);
                    }
                }

                result = Ok(recipe.Map(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Post([FromBody] Recipe recipe)
        {
            IActionResult result;

            try
            {
                var recipes = this.recipeService.Get(this.dbSettings);
                if (recipes.Any(x => x.Name.Equals(recipe.Name, StringComparison.InvariantCultureIgnoreCase)))
                    throw new Exception($"Ya existe una receta con el nombre: { recipe.Name }");

                //if (Request.Form.Files != null && Request.Form.Files.Any())
                //{
                //    IFormFile file = Request.Form.Files[0];
                //    Stream fileStream = file.OpenReadStream();
                //    BinaryReader reader = new BinaryReader(fileStream);
                //    byte[] image = reader.ReadBytes((int)fileStream.Length);
                //    recipe.ImageBase64 = Convert.ToBase64String(image, 0, image.Length);
                //}

                var created = this.recipeService.Add(this.dbSettings, recipe);

                if (created.RecipeDetails != null && created.RecipeDetails.Any())
                {
                    var products = this.productService.Get(this.dbSettings).Where(x => x.Active).ToList();
                    foreach (var recipeDetail in created.RecipeDetails)
                    {
                        recipeDetail.Product = products.FirstOrDefault(x => x.Id == recipeDetail.ProductId);
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
        public IActionResult Put([FromRoute(Name = "id")] int recipeId, [FromBody] Recipe recipe)
        {
            IActionResult result;

            try
            {
                var recipes = this.recipeService.Get(this.dbSettings);
                if (recipes.Any(x => x.Id != recipeId && x.Name.Equals(recipe.Name, StringComparison.InvariantCultureIgnoreCase)))
                    throw new Exception($"Ya existe una receta con el nombre: { recipe.Name }");

                //if (Request.Form.Files != null && Request.Form.Files.Any())
                //{
                //    IFormFile file = Request.Form.Files[0];
                //    Stream fileStream = file.OpenReadStream();
                //    BinaryReader reader = new BinaryReader(fileStream);
                //    byte[] image = reader.ReadBytes((int)fileStream.Length);
                //    recipe.ImageBase64 = Convert.ToBase64String(image, 0, image.Length);
                //}

                var edited = this.recipeService.Edit(this.dbSettings, recipeId, recipe);

                edited.RecipeDetails = this.recipeService.Get(this.dbSettings, edited).ToList();

                if (edited.RecipeDetails != null && edited.RecipeDetails.Any())
                {
                    var products = this.productService.Get(this.dbSettings).Where(x => x.Active).ToList();
                    foreach (var recipeDetail in edited.RecipeDetails)
                    {
                        recipeDetail.Product = products.FirstOrDefault(x => x.Id == recipeDetail.ProductId);
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
        public IActionResult Delete([FromRoute(Name = "id")] int recipeId)
        {
            IActionResult result;

            try
            {
                result = Ok(this.recipeService.Delete(this.dbSettings, recipeId));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}
