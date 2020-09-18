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
    public class RoleController : ControllerBase
    {
        private IRoleService _roleService { get; set; }
        private RestaurantDatabaseSettings _dbSettings { get; set; }

        public RoleController(IRoleService roleService, IOptions<RestaurantDatabaseSettings> databaseContext)
        {
            this._roleService = roleService;
            this._dbSettings = databaseContext.Value;
        }

        /// <summary>
        /// This method returns all roles
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
                result = Ok(this._roleService.Get(this._dbSettings).MapAll(true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}