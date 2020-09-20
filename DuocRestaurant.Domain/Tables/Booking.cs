using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Booking : RestaurantTable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TableId { get; set; }
        public DateTime Date { get; set; }
        public bool Active { get; set; }

        public User User { get; set; }
        public Table Table { get; set; }

        public struct ColumnNames
        {
            public const string Id = "Id";
            public const string UserId = "UsuarioId";
            public const string TableId = "MesaId";
            public const string Date = "Fecha";
            public const string Active = "Estado";
        }

        public override JObject Map(RestaurantDatabaseSettings ctx, bool customMap = true)
        {
            dynamic result = base.Map(ctx, customMap);

            if (customMap)
            {
                if (this.User != null)
                    result.User = this.User.Map(ctx, false);

                if (this.Table != null)
                    result.Table = this.Table.Map(ctx, false);
            }

            return result;
        }
    }
}
