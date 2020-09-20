using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface IProductTypeService
    {
        IList<ProductType> Get(RestaurantDatabaseSettings ctx);
        ProductType Get(RestaurantDatabaseSettings ctx, int productTypeId);
    }
}
