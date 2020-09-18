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

        public Role Role { get; set; }

        public struct ColumnNames
        {
            public const string Id = "Id";
            public const string RoleId = "RolId";
            public const string Name = "Nombre";
            public const string LastName = "Apellido";
            public const string Email = "Correo";
            public const string Password = "Contrasena";
            public const string Phone = "Telefono";
            public const string Address = "Direccion";
        }

        public override JObject Map(bool customMap = true)
        {
            dynamic result = base.Map(customMap);

            if (customMap)
            {
                if (this.Role != null)
                    result.Role = this.Role.Map(false);
            }

            return result;
        }
    }
}
