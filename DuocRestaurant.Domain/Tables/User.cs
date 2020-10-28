using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class User : RestaurantTable
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool Active { get; set; }

        public Role Role { get; set; }

        public struct ColumnNames
        {
            public const string Id = "Id";
            public const string RoleId = "RolId";
            public const string Name = "Nombre";
            public const string LastName = "Apellido";
            public const string Email = "Email";
            public const string Password = "Contrasena";
            public const string Phone = "Telefono";
            public const string Address = "Direccion";
            public const string Active = "Activo";
        }

        public override JObject Map(RestaurantDatabaseSettings ctx, bool customMap = true)
        {
            dynamic result = base.Map(ctx, customMap);

            if (!string.IsNullOrWhiteSpace(this.Password))
                result.Password = null;

            if (customMap)
            {
                if (this.Role != null)
                    result.Role = this.Role.Map(ctx, false);
            }

            return result;
        }
    }
}
