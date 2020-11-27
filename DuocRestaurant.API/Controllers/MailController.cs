using API.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DuocRestaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private IMailService mailService { get; set; }

        public MailController(IMailService mailService)
        {
            this.mailService = mailService;
        }

        [ActionName("SendMail")]
        [Route("[action]")]
        public IActionResult SendMail([FromBody] SendMailRequest request)
        {
            IActionResult result;

            try
            {
                this.mailService.SendMail(request.MailTo, request.Subject, request.Message);

                result = Ok();
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}
