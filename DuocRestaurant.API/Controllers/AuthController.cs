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
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService { get; set; }
        private RestaurantDatabaseSettings _dbSettings { get; set; }
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IConfiguration configuration, IOptions<RestaurantDatabaseSettings> databaseContext)
        {
            this._authService = authService;
            this._configuration = configuration;
            this._dbSettings = databaseContext.Value;
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

                    User user = _authService.SignIn(this._dbSettings, credentials.Username, credentials.Password);

                    if (user == null)
                        throw new Exception($"Credenciales incorrectas.");
                    
                    result = Ok(user.Map());
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
