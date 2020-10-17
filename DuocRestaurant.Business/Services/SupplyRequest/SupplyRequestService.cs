using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class SupplyRequestService : ISupplyRequestService
    {
        public SupplyRequest Add(RestaurantDatabaseSettings ctx, SupplyRequest supplyRequest)
        {
            throw new NotImplementedException();
        }

        public SupplyRequestDetail Add(RestaurantDatabaseSettings ctx, SupplyRequestDetail supplyRequestDetail)
        {
            throw new NotImplementedException();
        }

        public bool Delete(RestaurantDatabaseSettings ctx, int supplyRequestId)
        {
            throw new NotImplementedException();
        }

        public bool Delete(RestaurantDatabaseSettings ctx, SupplyRequestDetail supplyRequestDetail)
        {
            throw new NotImplementedException();
        }

        public SupplyRequest Edit(RestaurantDatabaseSettings ctx, int supplyRequestId, SupplyRequest supplyRequest)
        {
            throw new NotImplementedException();
        }

        public IList<SupplyRequest> Get(RestaurantDatabaseSettings ctx)
        {
            throw new NotImplementedException();
        }

        public IList<SupplyRequestDetail> Get(RestaurantDatabaseSettings ctx, SupplyRequest supplyRequest)
        {
            throw new NotImplementedException();
        }

        public SupplyRequest Get(RestaurantDatabaseSettings ctx, int supplyRequestId)
        {
            throw new NotImplementedException();
        }
    }
}
