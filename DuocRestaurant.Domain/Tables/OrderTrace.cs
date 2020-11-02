using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class OrderTrace : RestaurantTable
    {
        public int OrderId { get; set; }
        public int StateId { get; set; }
        public DateTime Date { get; set; }

        public const string TableName = "SeguimientoOrden";
        public struct ColumnNames
        {
            public const string OrderId = "OrdenId";
            public const string StateId = "EstadoId";
            public const string Date = "Fecha";
        }
    }
}
