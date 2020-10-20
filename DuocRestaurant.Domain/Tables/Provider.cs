using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Provider : RestaurantTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }

        public const string TableName = "Proveedor";
        public struct ColumnNames
        {
            public const string Id = "Id";
            public const string Name = "Nombre";
            public const string Address = "Direccion";
            public const string Phone = "Telefono";
            public const string Email = "Email";
            public const string Active = "Activo";
        }
    }
}
