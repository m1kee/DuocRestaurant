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
    public class BookingController : ControllerBase
    {
        private IBookingService bookingService { get; set; }
        private IUserService userService { get; set; }
        private ITableService tableService { get; set; }
        private RestaurantDatabaseSettings dbSettings { get; set; }

        public BookingController(IBookingService bookingService,
            IUserService userService,
            ITableService tableService,
            IOptions<RestaurantDatabaseSettings> databaseContext)
        {
            this.bookingService = bookingService;
            this.userService = userService;
            this.tableService = tableService;
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
                result = Ok(this.bookingService.Get(this.dbSettings).MapAll(this.dbSettings, true));
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
        public IActionResult Get([FromRoute(Name = "id")] int bookingId)
        {
            IActionResult result;

            try
            {
                result = Ok(this.bookingService.Get(this.dbSettings, bookingId).Map(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpPost]
        public IActionResult Post([FromBody]Booking booking)
        {
            IActionResult result;

            try
            {
                // validate that there is not other booking with the same table|date

                var created = this.bookingService.Add(this.dbSettings, booking);

                created.Table = this.tableService.Get(this.dbSettings, created.TableId);
                created.User = this.userService.Get(this.dbSettings, created.UserId);

                result = Ok(created.Map(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute(Name = "id")] int bookingId, [FromBody]Booking booking)
        {
            IActionResult result;

            try
            {
                // validate that there is not other booking with the same table|date

                var edited = this.bookingService.Edit(this.dbSettings, bookingId, booking);

                edited.Table = this.tableService.Get(this.dbSettings, edited.TableId);
                edited.User = this.userService.Get(this.dbSettings, edited.UserId);

                result = Ok(edited.Map(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute(Name = "id")] int bookingId)
        {
            IActionResult result;

            try
            {
                result = Ok(this.bookingService.Delete(this.dbSettings, bookingId));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}