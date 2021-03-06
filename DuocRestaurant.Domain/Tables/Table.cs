﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Table : RestaurantTable
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public bool Active { get; set; }
        public bool InUse { get; set; }
        public int? UserId { get; set; }

        public User User { get; set; }

        public const string TableName = "Mesa";
        public struct ColumnNames
        {
            public const string Id = "Id";
            public const string Number = "Numero";
            public const string Description = "Descripcion";
            public const string Capacity = "Capacidad";
            public const string Active = "Activa";
            public const string InUse = "EnUso";
            public const string UserId = "UsuarioId";
        }

        public override JObject Map(RestaurantDatabaseSettings ctx, bool customMap = true)
        {
            dynamic result = base.Map(ctx, customMap);

            if (customMap)
            {
                if (this.UserId != null && this.User != null)
                    result.User = this.User.Map(ctx, true);
            }

            return result;
        }
    }
}
