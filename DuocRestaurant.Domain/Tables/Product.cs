using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Product : RestaurantTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public int ProductTypeId { get; set; }
        public int Count { get; set; }
        public int MeasurementUnitId { get; set; }
        public decimal Price { get; set; }
        public int ProviderId { get; set; }
        public bool Active { get; set; }

        public Provider Provider { get; set; }
        public ProductType ProductType { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }

        public struct ColumnNames
        {
            public const string Id = "Id";
            public const string Name = "Nombre";
            public const string Details = "Detalle";
            public const string ProductTypeId = "TipoProductoId";
            public const string Count = "Cantidad";
            public const string MeasurementUnitId = "UnidadMedidaId";
            public const string Price = "Precio";
            public const string ProviderId = "ProveedorId";
            public const string Active = "Activo";
        }


        public override JObject Map(RestaurantDatabaseSettings ctx, bool customMap = false)
        {
            dynamic result = base.Map(ctx, customMap);

            if (customMap)
            {
                if (this.MeasurementUnit != null)
                {
                    result.MeasurementUnit = this.MeasurementUnit.Map(ctx, false);
                }

                if (this.Provider != null)
                {
                    result.Provider = this.Provider.Map(ctx, false);
                }

                if (this.ProductType != null)
                {
                    result.ProductType = this.ProductType.Map(ctx, false);
                }
            }

            return result;
        }
    }
}
