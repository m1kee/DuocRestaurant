using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Services;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

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
        [Route("[action]/{id:int}")]
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

        [HttpGet("{id}")]
        [ActionName("GetByCode")]
        [Route("[action]/{id:int}")]
        public IActionResult GetByCode([FromRoute(Name = "id")] int bookingCode)
        {
            IActionResult result;

            try
            {
                var booking = this.bookingService.GetByCode(this.dbSettings, bookingCode);
                if (booking == null)
                    throw new Exception($"No se encontró una reserva activa con el código: {bookingCode}");

                if (booking.State == Booking.BookingState.Expired)
                {
                    throw new Exception($"La reserva ha caducado ya que han pasado más de 15 minutos.");
                } 
                else
                {
                    DateTime now = DateTime.Now;
                    TimeSpan diff = now.Subtract(booking.Date);
                    if (diff.TotalMinutes < 0)
                    {
                        throw new Exception($"Su reserva está agendada para el día {booking.Date.ToString("D", System.Globalization.CultureInfo.CreateSpecificCulture("es-ES"))} a las {booking.Date.ToString("HH:mm")} hrs.");
                    }
                }

                result = Ok(booking.Map(this.dbSettings, true));
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
                var bookings = this.bookingService.Get(this.dbSettings);
                if (booking.Date.ToLocalTime() < DateTime.Now)
                {
                    throw new Exception($"No puedes crear una reserva para una fecha anterior a {DateTime.Now:dd-MM-yyyy HH:mm}.");
                }
                // verify inside my bookings
                else if (bookings.Any(x => x.UserId == booking.UserId && x.Date.Equals(booking.Date.ToLocalTime()) && x.Active))
                {
                    throw new Exception($"Ya tienes una reserva creada para el mismo horario en la misma fecha.");
                }
                // verify other people bookings
                else if (bookings.Any(x => x.TableId == booking.TableId && x.Date.Equals(booking.Date.ToLocalTime()) && x.Active))
                {
                    throw new Exception($"Ya existe una reserva creada para este horario y mesa.");
                }

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

        [ActionName("FilterBy")]
        [Route("[action]")]
        public IActionResult FilterBy([FromBody]JObject filters)
        {
            IActionResult result;

            try
            {
                var bookings = this.bookingService.Get(this.dbSettings);

                if (filters.ContainsKey("UserId"))
                {
                    int userId = Convert.ToInt32(filters.GetValue("UserId").ToString());

                    bookings = bookings.Where(x => x.UserId >= userId).ToList();
                }

                result = Ok(bookings.MapAll(this.dbSettings, true));
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}