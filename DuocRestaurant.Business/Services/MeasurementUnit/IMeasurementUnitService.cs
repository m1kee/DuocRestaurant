using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface IMeasurementUnitService
    {
        IList<MeasurementUnit> Get(RestaurantDatabaseSettings ctx);
        MeasurementUnit Get(RestaurantDatabaseSettings ctx, int measurementUnitId);
    }
}
