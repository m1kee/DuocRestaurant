using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface ISupplyRequestService
    {
        SupplyRequest Add(RestaurantDatabaseSettings ctx, SupplyRequest supplyRequest);
        SupplyRequestDetail Add(RestaurantDatabaseSettings ctx, SupplyRequestDetail supplyRequestDetail);
        SupplyRequest Edit(RestaurantDatabaseSettings ctx, int supplyRequestId, SupplyRequest supplyRequest);
        IList<SupplyRequest> Get(RestaurantDatabaseSettings ctx);
        IList<SupplyRequestDetail> Get(RestaurantDatabaseSettings ctx, SupplyRequest supplyRequest);
        SupplyRequest Get(RestaurantDatabaseSettings ctx, int supplyRequestId);
        bool Delete(RestaurantDatabaseSettings ctx, int supplyRequestId);
        bool Delete(RestaurantDatabaseSettings ctx, SupplyRequestDetail supplyRequestDetail);
    }
}
