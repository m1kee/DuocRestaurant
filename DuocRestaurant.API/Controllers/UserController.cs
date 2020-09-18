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
        private IUserService _userService { get; set; }
        private RestaurantDatabaseSettings _dbSettings { get; set; }

        public UserController(IUserService userService, IOptions<RestaurantDatabaseSettings> databaseContext)
        {
            this._userService = userService;
            this._dbSettings = databaseContext.Value;
        }

        /// <summary>
        /// This method returns all tables
        /// </summary>
        /// <returns>List<User></returns>
        [HttpGet]
        [ActionName("GetAll")]
        [Route("[action]")]
        public IActionResult Get()
        {
            IActionResult result;

            try
            {
                result = Ok(this._userService.Get(this._dbSettings).MapAll(true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// This method returns a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>User</returns>
        [HttpGet]
        [ActionName("GetById")]
        [Route("[action]")]
        public IActionResult Get([FromRoute(Name = "id")] int userId)
        {
            IActionResult result;

            try
            {
                result = Ok(this._userService.Get(this._dbSettings, userId).Map(true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// This method inserts a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Inserted User</returns>
        [HttpPost]
        public IActionResult Post([FromBody]User user)
        {
            IActionResult result;

            try
            {
                result = Ok(this._userService.Add(this._dbSettings, user));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// This method update a user by it's Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        /// <returns>Updated User</returns>
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute(Name = "id")] int userId, [FromBody]User user)
        {
            IActionResult result;

            try
            {
                // TODO: validate things
                result = Ok(this._userService.Edit(this._dbSettings, userId, user));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// this method deletes a user by it's Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Deleted User</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute(Name = "id")] int userId)
        {
            IActionResult result;

            try
            {
                // TODO: validate things
                result = Ok(this._userService.Delete(this._dbSettings, userId));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}