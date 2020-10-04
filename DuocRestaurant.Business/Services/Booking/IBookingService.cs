using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface IBookingService
    {
        IList<Booking> Get(RestaurantDatabaseSettings ctx);
        Booking Get(RestaurantDatabaseSettings ctx, int bookingId);
        Booking GetByCode(RestaurantDatabaseSettings ctx, int bookingCode);
        Booking Add(RestaurantDatabaseSettings ctx, Booking booking);
        Booking Edit(RestaurantDatabaseSettings ctx, int bookingId, Booking booking);
        bool Delete(RestaurantDatabaseSettings ctx, int bookingId);
    }
}
