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
    public class UserController : ControllerBase
    {
        private IUserService userService { get; set; }
        private RestaurantDatabaseSettings dbSettings { get; set; }

        public UserController(IUserService userService, IOptions<RestaurantDatabaseSettings> databaseContext)
        {
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
                result = Ok(this.userService.Get(this.dbSettings).MapAll(true));
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
        public IActionResult Get([FromRoute(Name = "id")] int userId)
        {
            IActionResult result;

            try
            {
                result = Ok(this.userService.Get(this.dbSettings, userId).Map(true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpPost]
        public IActionResult Post([FromBody]User user)
        {
            IActionResult result;

            try
            {
                var users = this.userService.Get(this.dbSettings);
                if (users.Any(x => x.Email.Equals(user.Email, StringComparison.InvariantCultureIgnoreCase)))
                    throw new Exception($"Ya existe una usuario con el correo: { user.Email }");

                result = Ok(this.userService.Add(this.dbSettings, user));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute(Name = "id")] int userId, [FromBody]User user)
        {
            IActionResult result;

            try
            {
                var users = this.userService.Get(this.dbSettings);
                if (users.Any(x => x.Id != userId && x.Email.Equals(user.Email, StringComparison.InvariantCultureIgnoreCase)))
                    throw new Exception($"Ya existe una usuario con el correo: { user.Email }");

                result = Ok(this.userService.Edit(this.dbSettings, userId, user));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute(Name = "id")] int userId)
        {
            IActionResult result;

            try
            {
                result = Ok(this.userService.Delete(this.dbSettings, userId));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}