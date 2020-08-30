using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DuocRestaurant.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DefaultController : ControllerBase
    {
        // GET: default
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hola mundo");
        }
    }
}
