ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY hh24:mi:ss';

DROP TABLE DetalleReceta;
DROP TABLE Receta;
DROP TABLE Reserva;
DROP TABLE Mesa;
DROP TABLE Usuario;
DROP TABLE Rol;
DROP TABLE DetallePedidoInsumo;
DROP TABLE PedidoInsumo;
DROP TABLE EstadoPedidoInsumo;
DROP TABLE Producto;
DROP TABLE TipoProducto;
DROP TABLE UnidadMedida;
DROP TABLE Proveedor;

-- SCHEMA CREATION
CREATE TABLE Rol (
    Id INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
    Descripcion VARCHAR2(20) NOT NULL,
    CONSTRAINT PK_Rol PRIMARY KEY (Id)
);

CREATE TABLE Usuario (
    Id INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
    RolId INTEGER NOT NULL,
    Nombre VARCHAR2(30) NOT NULL,
    Apellido VARCHAR2(30) NOT NULL,
    Email VARCHAR2(100) NOT NULL,
    Contrasena VARCHAR2(64) NOT NULL,
    Telefono VARCHAR2(15),
    Direccion VARCHAR2(200),
    Activo NUMBER(1),
    CONSTRAINT PK_Usuario PRIMARY KEY (Id),
    CONSTRAINT FK_Usuario_Rol FOREIGN KEY (RolId) REFERENCES Rol(Id)
);
    
CREATE TABLE Mesa (
    Id INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
    Numero INTEGER NOT NULL,
    Descripcion VARCHAR2(30),
    Capacidad INTEGER,
    Activa NUMBER(1) NOT NULL,
    EnUso NUMBER(1) NOT NULL,
    UsuarioId NUMBER(1) NULL,
    CONSTRAINT PK_Mesa PRIMARY KEY (Id)
);

CREATE TABLE Reserva (
    Id INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
    Codigo VARCHAR2(50) NOT NULL, -- UsuarioId + MesaId + Fecha(YYYYMMDDHH)
    UsuarioId INTEGER NOT NULL,
    MesaId INTEGER NOT NULL,
    Fecha DATE NOT NULL,
    Comensales INTEGER NOT NULL,
    Estado NUMBER(1) NOT NULL,
    CONSTRAINT PK_Reserva PRIMARY KEY (Id),
    CONSTRAINT FK_Reserva_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id),
    CONSTRAINT FK_Reserva_Mesa FOREIGN KEY (MesaId) REFERENCES Mesa(Id)
);
    
CREATE TABLE TipoProducto(
    Id INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
    Descripcion VARCHAR2(30) NOT NULL,
    CONSTRAINT PK_TipoProducto PRIMARY KEY (Id)
);

CREATE TABLE Proveedor (
    Id INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
    Nombre VARCHAR2(30) NOT NULL,
    Direccion VARCHAR2(200) NOT NULL,
    Telefono VARCHAR2(15) NOT NULL,
    Email VARCHAR2(150) NOT NULL,
    Activo NUMBER(1) NOT NULL,
    CONSTRAINT PK_Proveedor PRIMARY KEY (Id)
);

CREATE TABLE UnidadMedida (
    Id INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
    Codigo VARCHAR2(30) NOT NULL,
    Descripcion VARCHAR2(30) NOT NULL,
    CONSTRAINT PK_UnidadMedida PRIMARY KEY (Id)
);

CREATE TABLE Producto (
    Id INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
    Nombre VARCHAR2(150) NOT NULL,
    Detalle VARCHAR2(500),
    TipoProductoId INTEGER NOT NULL,
    Cantidad INTEGER NOT NULL,
    UnidadMedidaId INTEGER NOT NULL,
    Precio decimal NOT NULL,
    ProveedorId INTEGER NOT NULL,
    Activo NUMBER(1) NOT NULL,
    CONSTRAINT PK_Producto PRIMARY KEY (Id),
    CONSTRAINT FK_Producto_TipoProducto FOREIGN KEY (TipoProductoId) REFERENCES TipoProducto(Id),
    CONSTRAINT FK_Producto_UnidadMedida FOREIGN KEY (UnidadMedidaId) REFERENCES UnidadMedida(Id),
    CONSTRAINT FK_Producto_Proveedor FOREIGN KEY (ProveedorId) REFERENCES Proveedor(Id)
);

CREATE TABLE Receta (
    Id INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
    Nombre VARCHAR2(75) NOT NULL,
    Precio INTEGER NOT NULL,
    Detalle VARCHAR2(150),
    TiempoPreparacion NUMBER(8, 2) NOT NULL,
    Imagen BLOB NULL,
    Activa NUMBER(1) NOT NULL,
    CONSTRAINT PK_Receta PRIMARY KEY (Id)
);

CREATE TABLE DetalleReceta(
    RecetaId INTEGER NOT NULL,
    ProductoId INTEGER NOT NULL,
    Cantidad NUMBER(8,3) NOT NULL,
    Activo NUMBER(1) NOT NULL,
    CONSTRAINT PK_DetalleReceta PRIMARY KEY (RecetaId, ProductoId),
    CONSTRAINT FK_DetalleReceta_Receta FOREIGN KEY (RecetaId) REFERENCES Receta(Id),
    CONSTRAINT FK_DetalleReceta_Producto FOREIGN KEY (ProductoId) REFERENCES Producto(Id)
);

CREATE TABLE EstadoPedidoInsumo(
    Id INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
    Descripcion VARCHAR2(20) NOT NULL,
    CONSTRAINT PK_EstadoPedidoInsumo PRIMARY KEY (Id)
);  

CREATE TABLE PedidoInsumo(
    Id INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
    Codigo VARCHAR2(50) NOT NULL, -- UsuarioId + MesaId + Fecha(YYYYMMDDHH)
    ProveedorId INTEGER NOT NULL,
    Fecha DATE NOT NULL,
    EstadoPedidoId INTEGER NOT NULL,
    Activo NUMBER(1) NOT NULL,
    CONSTRAINT PK_PedidoInsumo PRIMARY KEY (Id),
    CONSTRAINT FK_PedidoInsumo_Proveedor FOREIGN KEY (ProveedorId) REFERENCES Proveedor(Id),
    CONSTRAINT FK_PedidoInsumo_EstadoPedidoInsumo FOREIGN KEY (EstadoPedidoId) REFERENCES EstadoPedidoInsumo(Id)
); 

CREATE TABLE DetallePedidoInsumo(
    PedidoInsumoId INTEGER NOT NULL,
    ProductoId INTEGER NOT NULL,
    Cantidad INTEGER NOT NULL,
    Activo NUMBER(1) NOT NULL,
    CONSTRAINT PK_DetallePedidoInsumo PRIMARY KEY (PedidoInsumoId, ProductoId),
    CONSTRAINT FK_DetallePedidoInsumo_PedidoInsumo FOREIGN KEY (PedidoInsumoId) REFERENCES PedidoInsumo(Id),
    CONSTRAINT FK_DetallePedidoInsumo_Producto FOREIGN KEY (ProductoId) REFERENCES Producto(Id)
); 

-- INSERTS
INSERT INTO Rol (Descripcion) VALUES ('Administrador');
INSERT INTO Rol (Descripcion) VALUES ('Bodega');
INSERT INTO Rol (Descripcion) VALUES ('Finanzas');
INSERT INTO Rol (Descripcion) VALUES ('Cocina');
INSERT INTO Rol (Descripcion) VALUES ('Garzon');
INSERT INTO Rol (Descripcion) VALUES ('Cliente');
INSERT INTO Rol (Descripcion) VALUES ('Recepción');
INSERT INTO Rol (Descripcion) VALUES ('Mesa');

INSERT INTO Usuario (RolId, Nombre, Apellido, Email, Contrasena, Telefono, Direccion, Activo)
VALUES (1,'Segismundo', 'Morat', 'Segi_Mora168@SigloXXI.cl', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56922223760', 'Victoria,2251,Maipu', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Email, Contrasena, Telefono, Direccion, Activo)
VALUES (2,'Laura', 'Paz', 'l_paz1999@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56967239551', 'Calama,6028,Pudahuel', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Email, Contrasena, Telefono, Direccion, Activo)
VALUES (3,'Diego', 'Torres', 'diegox_ushiha@hotmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+569954670302', 'Jorge Hunneus,754,Quinta Normal', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Email, Contrasena, Telefono, Direccion, Activo)
VALUES (4,'Valentina', 'Pincheira', 'valery-uwu@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56978902121', 'Gamero,529,Independencia', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Email, Contrasena, Telefono, Direccion, Activo)
VALUES (4,'Henry', 'Zamora', 'dracula_darks@hotmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56934523033', 'Gamero,567,Independencia', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Email, Contrasena, Telefono, Direccion, Activo)
VALUES (4,'Mario', 'Pino', 'mariop1993@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56923408485', 'San Martin,575,Santiago', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Email, Contrasena, Telefono, Direccion, Activo)
VALUES (4,'Claudia', 'Salfate', 'salfate_clau94@outlook.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56943407675', 'Fermin Vivaceta,502,Independencia', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Email, Contrasena, Telefono, Direccion, Activo)
VALUES (5,'Perla', 'San Martin', 'gema-ruby3@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56940331111', 'Recoleta,598,Recoleta', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Email, Contrasena, Telefono, Direccion, Activo)
VALUES (5,'Alberto', 'Rivera', 'albersoto@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56925340555', 'Dominicana,200,Recoleta', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Email, Contrasena, Telefono, Direccion, Activo)
VALUES (5,'Julia', 'Aries', 'juliana-kawaii@hotmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56934506653', 'Francisco Bilbao,6052,Providencia', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Email, Contrasena, Telefono, Direccion, Activo)
VALUES (5,'Ares', 'Martines', 'fe4-jp@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56949492332', 'Gamero,531,Independencia', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Email, Contrasena, Telefono, Direccion, Activo)
VALUES (5,'Eubanz', 'Brando', 'banz-87@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56922523042', 'Victoria,666,Maipu', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Email, Contrasena, Telefono, Direccion, Activo)
VALUES (5,'Pablo', 'Mamani', 'perezpe3@hotmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56978924721', 'Duble almeyda,3755,Nuñoa', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Email, Contrasena, Telefono, Direccion, Activo)
VALUES (6,'Maria', 'Bella', 'maripe@yopmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56975797876', '5 de abril,2220,Maipu', 0);
INSERT INTO Usuario (RolId, Nombre, Apellido, Email, Contrasena, Telefono, Direccion, Activo)
VALUES (7,'Recepción', 'Siglo XXI', 'recepcion@sigloxxi.cl', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56978221199', '3 Poniente, 2242, Maipu', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Email, Contrasena, Telefono, Direccion, Activo)
VALUES (8,'Mesa 1', '', 'mesa-1@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56900004533', '4 Poniente 5044, Maipu', 1);

INSERT INTO Mesa (Numero, Descripcion, Capacidad, EnUso, Activa, UsuarioId) VALUES (1,'mesa interior',2,0,1, NULL);
INSERT INTO Mesa (Numero, Descripcion, Capacidad, EnUso, Activa, UsuarioId) VALUES (2,'mesa interior',4,0,1, NULL);
INSERT INTO Mesa (Numero, Descripcion, Capacidad, EnUso, Activa, UsuarioId) VALUES (3,'mesa interior',4,0,1, NULL);
INSERT INTO Mesa (Numero, Descripcion, Capacidad, EnUso, Activa, UsuarioId) VALUES (4,'mesa tv',6,0,1, NULL);
INSERT INTO Mesa (Numero, Descripcion, Capacidad, EnUso, Activa, UsuarioId) VALUES (5,'barra bar',1,0,1, NULL);
INSERT INTO Mesa (Numero, Descripcion, Capacidad, EnUso, Activa, UsuarioId) VALUES (6,'barra bar',1,0,1, NULL);
INSERT INTO Mesa (Numero, Descripcion, Capacidad, EnUso, Activa, UsuarioId) VALUES (7,'mesa exterior',4,0,1, NULL);

INSERT INTO TipoProducto (Descripcion) VALUES ('Insumo');
INSERT INTO TipoProducto (Descripcion) VALUES ('Consumible');

INSERT INTO Proveedor (Nombre, Direccion, Telefono, Email, Activo) VALUES ('Concha y Toro', 'Viña Concha y Toro, Pirque', '+56966623231', 'contacto.vtoro@conchaytoro.cl', 1);
INSERT INTO Proveedor (Nombre, Direccion, Telefono, Email, Activo) VALUES ('Santa Helena', 'Viña Santa Helena, Providencia', '+56955523231', 'contacto.helena@sthelena.cl', 1);
INSERT INTO Proveedor (Nombre, Direccion, Telefono, Email, Activo) VALUES ('Carne ST Maria', 'Peña #885, Curico', '+56955423231', 'carnitas@mariap.cl', 1);
INSERT INTO Proveedor (Nombre, Direccion, Telefono, Email, Activo) VALUES ('Granja Feliz', 'Av. Victoria #2351, Maipu', '+56962623231', 'contacto.huevos@gfeliz.cl', 1);
INSERT INTO Proveedor (Nombre, Direccion, Telefono, Email, Activo) VALUES ('Coca-Cola', 'Renca', '+56912323231', 'contacto-clp@cocacola.cl', 1);
INSERT INTO Proveedor (Nombre, Direccion, Telefono, Email, Activo) VALUES ('Evercrips', 'Cerrillos #999, Cerrillos', '+56923423231', 'contacto.evrcrips-clp@ccu.cl', 1);
INSERT INTO Proveedor (Nombre, Direccion, Telefono, Email, Activo) VALUES ('La pagoda cervecera', '4 Ponientes #9454, Maipu', '+56982233231', 'contacto.pcervi@pagodaclp.cl', 1);
INSERT INTO Proveedor (Nombre, Direccion, Telefono, Email, Activo) VALUES ('Nueva Arabia', 'Av. San Martin #578, Viña', '+56966622221', 'contacto.nachile@nuevaarabia.cl', 1);
INSERT INTO Proveedor (Nombre, Direccion, Telefono, Email, Activo) VALUES ('El Pollo Granjero', 'Talca', '+56966273731', 'byron_ezel@gmail.com', 1);
INSERT INTO Proveedor (Nombre, Direccion, Telefono, Email, Activo) VALUES ('Viejo Verde', 'Rancagua', '+56966625555', 'maria_sol98@hotmail.com', 1);
INSERT INTO Proveedor (Nombre, Direccion, Telefono, Email, Activo) VALUES ('Tio Aceite', 'El puerto #2, Valparaiso', '+56911111111', 'juanandrez@tioaceite.cl', 1);
INSERT INTO Proveedor (Nombre, Direccion, Telefono, Email, Activo) VALUES ('Moria', 'Rancagua', '+56933223731', 'deyanira-mora@moria.cl', 1);
INSERT INTO Proveedor (Nombre, Direccion, Telefono, Email, Activo) VALUES ('Carnes Madagascar', 'Putre #234, Arica', '+56948294231', 'julian.mort@gmail.com', 1);

INSERT INTO UnidadMedida (Codigo, Descripcion) VALUES ('kg', 'kilogramo');
INSERT INTO UnidadMedida (Codigo, Descripcion) VALUES ('gr', 'gramo');
INSERT INTO UnidadMedida (Codigo, Descripcion) VALUES ('un', 'unidad');
INSERT INTO UnidadMedida (Codigo, Descripcion) VALUES ('lt', 'litro');
INSERT INTO UnidadMedida (Codigo, Descripcion) VALUES ('ml', 'mililitro');

INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Casillero del diablo 1L','vino tinto',2,34,4,15000,1,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Casillero del diablo 5L','vino tinto',2,29,4,15000,1,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Surise 2L','merlot',2,21,4,15000,1,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Gran reserva series riveras 1.5L','sauvignon blanc',2,15,4,30000,1,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Santa helena tinto 2L','vino tinto',1,119,4,1500,2,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Santa helena blanco 2L','vino blanco',1,38,4,1500,2,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Carne molida cerdo 5kg','carne roja',1,41,1,4000,3,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Costillar de cerdo 3kg','carne roja',1,98,1,10000,3,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Filete de cerdo 15kg','carne roja',1,6,1,8000,3,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Choripanes 2kg','carne roja',1,8,1,4000,3,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Lomo vacuno 15kg','carne roja',1,78,1,10000,3,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Pierna vacuno 10kg','carne roja',1,89,1,14000,3,0);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Hamburguesa vacuno 20u','carne roja',1,66,3,8000,3,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Huevo gallina 30u','huevos',1,59,3,2000,4,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Alitas de pollo 10kg','carne roja',1,116,1,16000,4,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Nuggets de pollo 5kg','carne roja',2,92,1,4000,4,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Pechuga de pollo 3kg','carne roja',1,64,1,18000,4,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES (' Coca-cola 500ml','bebida',2,55,5,18000,5,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Coca-cola light 500ml','bebida',2,68,5,6000,5,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Coca-cola zero 500ml','bebida',2,133,5,8000,5,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Fanta 500ml','bebida',2,60,5,16000,5,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Sprite 500ml','bebida',2,59,5,2000,5,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Quatro 500ml','bebida',2,147,5,14000,5,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Nectar andina durazno 300ml','bebida',2,129,5,2000,5,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Nectar andina naranja 300ml','bebida',2,140,5,6000,5,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Mani salado 500g','fruto seco',2,56,2,2000,6,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Mani sin sal 500g','fruto seco',2,33,2,18000,6,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Almendras 500g','fruto seco',2,72,2,14000,6,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Nueces 500g','fruto seco',2,56,2,12000,6,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Kunzman torobayo 500ml','cerveza',2,45,4,14000,7,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('kunzman bock 500ml','cerveza',2,94,4,12000,7,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Kunzman miel 500ml','cerveza',2,19,4,14000,7,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Aguamiel de la casa 500ml','cerveza',2,126,4,2000,7,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Cerveza aliento de dragon 500ml','cerveza',2,97,4,14000,7,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Autral lager 500ml','cerveza',2,21,4,10000,7,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Cristal 500ml','cerveza',2,90,4,12000,7,0);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Sol 500ml','cerveza',2,124,4,4000,7,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Agua trol 500ml','cerveza',2,19,4,10000,7,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Café colombia 10kg','café',2,6,1,6000,7,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Sal 5kg','aliño',1,97,1,2000,8,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Pimienta 5kg','aliño',1,122,1,16000,8,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Canela 1kg','aliño',1,122,1,18000,8,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Comino 1kg','aliño',1,74,1,2000,8,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Azucar 10kg','aliño',1,34,1,16000,8,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Té negro 100u','té',2,77,3,18000,8,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Té verde 100u','té',2,42,3,2000,8,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Té blanco 100u','té',2,98,3,8000,8,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Té rojo 100u','té',2,138,3,16000,8,0);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Curry barra 50u','aliño',1,36,3,10000,8,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Huevos de codorniz 24u','huevos',1,119,3,6000,9,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Nuggets de codorniz 1kg','carne roja',2,88,1,4000,9,0);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('champiñones 100u','verdura',1,74,3,16000,10,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Pimiento rojo 20u','verdura',1,85,3,18000,10,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Pimiento amarillo 20u','verdura',1,71,3,12000,10,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Pimiento verde 20u','verdura',1,57,3,12000,10,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Choclos 10u','verdura',1,116,3,13450,10,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Tomate 15u','verdura',1,25,3,12000,10,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Limones 30u','verdura',1,93,3,2000,10,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Ajo 14u','verdura',1,18,3,2340,10,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Cebolla 20u','verdura',1,128,3,4000,10,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Zanahoria 20u','verdura',1,92,3,1200,10,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Papas 20kg','verdura',1,129,1,18000,10,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Camotes 4kg','verdura',1,84,1,16000,10,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Cilantro 15u','verdura',1,24,3,14000,10,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Peregil 15u','verdura',1,21,3,4000,10,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Aceite girasol 2L','aceite',1,5,4,1000,11,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Aceite oliva extra virgen 1L','aceite',1,41,4,6800,11,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Vinagre tinto 500ml','vinagre',1,91,5,900,11,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Vinagre blanco 500ml','vinagre',1,133,5,900,11,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Filete de salmon 5u','pescado',1,62,3,7890,11,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Arroz tucapel 1kg','grano',1,16,1,8000,12,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Cuscus 1kg','grano',1,108,1,10000,12,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Kinoa 1kg','grano',1,121,1,2000,12,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Harinapam 5kg','harina',1,29,1,12000,12,0);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Harina blanca 10kg','harina',1,36,1,8000,12,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Levadura 1kg','levadura',1,111,1,10000,12,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Tinta comestible 3u','tinta',1,146,3,8000,12,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Helado fresa 2L','helado',2,49,4,4600,13,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Helado chocolate 2L','helado',2,113,4,4600,13,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Helado Vainilla 2L','helado',2,19,4,4600,13,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Hamburguesa vegana 20u','producto vegano',1,24,3,16000,13,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Leche blanca 2L','leche',1,71,4,1000,13,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Leche descremada 2L','leche',1,140,4,1000,13,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Leche Semi-descreamada 2L','leche',1,34,4,1000,13,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Leche vegana coco 1L','producto vegano',1,19,4,1000,13,0);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Lecha vegana almendras 1L','producto vegano',1,20,4,1000,13,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Mayonesa Hellmans 1L','complemento',2,147,4,3000,13,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Ketchup Hellmans 1L','complemento',2,53,4,3000,13,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Mostaza Hellmans 1L','complemento',2,147,4,3000,13,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Mayonesa Vegana 1L','producto vegano',2,38,4,3600,13,1);
INSERT INTO Producto (Nombre, Detalle, TipoProductoId, Cantidad, UnidadMedidaId, Precio, ProveedorId, Activo) VALUES ('Tabla de quesos variedades 1u','queso',2,21,3,18000,13,1);

INSERT INTO Receta (Nombre, Precio, Detalle, TiempoPreparacion, Imagen, Activa) VALUES ('Arroz del comienzo',2500,'Un platillo simple con verduras y huevo de codorniz', 20, NULL, 1);
INSERT INTO Receta (Nombre, Precio, Detalle, TiempoPreparacion, Imagen, Activa) VALUES ('Queque zanahoria',900,'Porcion del clasico queque de zanahorias', 40, NULL, 1);

INSERT INTO DetalleReceta (RecetaId, ProductoId, Cantidad, Activo) VALUES (1,71,0.100, 1);
INSERT INTO DetalleReceta (RecetaId, ProductoId, Cantidad, Activo) VALUES (1,50,1, 1);
INSERT INTO DetalleReceta (RecetaId, ProductoId, Cantidad, Activo) VALUES (1,54,0.5, 1);
INSERT INTO DetalleReceta (RecetaId, ProductoId, Cantidad, Activo) VALUES (1,59,0.1, 1);
INSERT INTO DetalleReceta (RecetaId, ProductoId, Cantidad, Activo) VALUES (1,66,0.03, 1);
INSERT INTO DetalleReceta (RecetaId, ProductoId, Cantidad, Activo) VALUES (1,40,0.030, 1);
INSERT INTO DetalleReceta (RecetaId, ProductoId, Cantidad, Activo) VALUES (2,75,0.100, 1);
INSERT INTO DetalleReceta (RecetaId, ProductoId, Cantidad, Activo) VALUES (2,61,1, 1);
INSERT INTO DetalleReceta (RecetaId, ProductoId, Cantidad, Activo) VALUES (2,82,0.050, 1);
INSERT INTO DetalleReceta (RecetaId, ProductoId, Cantidad, Activo) VALUES (2,44,0.020, 1);
INSERT INTO DetalleReceta (RecetaId, ProductoId, Cantidad, Activo) VALUES (2,76,0.005, 1);


INSERT INTO EstadoPedidoInsumo (Descripcion) VALUES ('Creada');
INSERT INTO EstadoPedidoInsumo (Descripcion) VALUES ('Enviada');
INSERT INTO EstadoPedidoInsumo (Descripcion) VALUES ('Confirmada');
INSERT INTO EstadoPedidoInsumo (Descripcion) VALUES ('Rechazada');

COMMIT;
