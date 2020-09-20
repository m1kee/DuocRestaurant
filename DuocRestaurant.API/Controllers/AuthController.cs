using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Business.Services;
using Domain;
using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;

namespace DuocRestaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService authService { get; set; }
        private RestaurantDatabaseSettings dbSettings { get; set; }

        public AuthController(IAuthService authService, IOptions<RestaurantDatabaseSettings> databaseContext)
        {
            this.authService = authService;
            this.dbSettings = databaseContext.Value;
        }

        [HttpPost]
        [ActionName("SignIn")]
        [Route("[action]")]
        public IActionResult SignIn([FromBody]SignInRequest credentials)
        {
            IActionResult result = null;
            try
            {
                if (credentials != null)
                {
                    if (string.IsNullOrWhiteSpace(credentials.Username))
                        throw new Exception($"Debe ingresar el nombre de usuario.");

                    if (string.IsNullOrWhiteSpace(credentials.Password))
                        throw new Exception($"Debe ingresar la contraseña.");

                    credentials.Password = EncryptHelper.SHA256(credentials.Password);

                    User user = authService.SignIn(this.dbSettings, credentials.Username, credentials.Password);

                    if (user == null)
                        throw new Exception($"Credenciales incorrectas.");

                    result = Ok(user.Map(true));
                }
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}
