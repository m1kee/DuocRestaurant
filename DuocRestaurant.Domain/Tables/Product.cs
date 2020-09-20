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

    }
}
