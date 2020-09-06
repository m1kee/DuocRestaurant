using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class User : RestaurantTable
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }

        public struct ColumnNames
        {
            public const string Id = "Id";
            public const string RolId = "RolId";
            public const string Nombre = "Nombre";
            public const string Apellido = "Apellido";
            public const string Correo = "Correo";
            public const string Contrasena = "Contrasena";
            public const string Telefono = "Telefono";
            public const string Direccion = "Direccion";
        }
    }
}
