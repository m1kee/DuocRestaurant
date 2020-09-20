ALTER SESSION SET nls_date_format = 'DD/MM/YYYY hh24:mi:ss';

DROP TABLE Producto;
DROP TABLE TipoProducto;
DROP TABLE Reserva;
DROP TABLE Proveedor;
DROP TABLE UnidadMedida;
DROP TABLE Mesa;
DROP TABLE Usuario;
DROP TABLE Rol;

CREATE TABLE Rol (
    Id integer generated always as identity (start with 1 increment by 1),
    Descripcion nvarchar2(20) not null
);

alter table Rol add (
    CONSTRAINT PK_Rol primary key(Id)
);

create table Usuario (
    Id integer generated always as identity (start with 1 increment by 1),
    RolId integer not null,
    Nombre nvarchar2(30) not null,
    Apellido nvarchar2(30) not null,
    Correo nvarchar2(100) not null,
    Contrasena nvarchar2(64) not null,
    Telefono nvarchar2(15),
    Direccion nvarchar2(200),
    Activo number(1)
);

alter table Usuario add (
    CONSTRAINT PK_Usuario primary key(Id)
);

alter table Usuario add constraint FK_Usuario_Rol
    foreign key (RolId) references Rol(Id);
    
create table Mesa (
    Id integer generated always as identity (start with 1 increment by 1),
    Numero integer not null,
    Descripcion nvarchar2(30),
    Capacidad integer,
    Activa number(1) not null,
    EnUso number(1) not null
);

alter table Mesa add(
    CONSTRAINT PK_Mesa primary key(Id)
);

CREATE TABLE Reserva (
    Id integer generated always as identity (start with 1 increment by 1),
    UsuarioId integer NOT NULL,
    MesaId integer NOT NULL,
    Fecha DATE NOT NULL,
    Estado NUMBER(1) NOT NULL
);

alter table Reserva add(
    CONSTRAINT PK_Reserva primary key(Id)
);

alter table Reserva add constraint FK_Reserva_Usuario
    foreign key (UsuarioId) references Usuario(Id);
    
alter table Reserva add constraint FK_Reserva_Mesa
    foreign key (MesaId) references Mesa(Id);
    
CREATE TABLE TipoProducto(
    Id integer generated always as identity (start with 1 increment by 1),
    Descripcion NVARCHAR2(30) not null
);

alter table TipoProducto add(
    CONSTRAINT PK_TipoProducto primary key(Id)
);

CREATE TABLE Proveedor (
    Id integer generated always as identity (start with 1 increment by 1),
    Nombre NVARCHAR2(30) not null,
    Direccion NVARCHAR2(200) not null,
    Telefono NVARCHAR2(15) not null,
    Email NVARCHAR2(150) not null,
    Activo number(1) not null
);

alter table Proveedor add (
    CONSTRAINT PK_Proveedor primary key(Id)
);

CREATE TABLE UnidadMedida (
    Id integer generated always as identity (start with 1 increment by 1),
    Codigo NVARCHAR2(30) not null,
    Descripcion NVARCHAR2(30) not null
);

alter table UnidadMedida add(
    CONSTRAINT PK_UnidadMedida primary key(Id)
);

CREATE TABLE Producto (
    Id integer generated always as identity (start with 1 increment by 1),
    Nombre NVARCHAR2(150) not null,
    Detalle NVARCHAR2(500),
    TipoProductoId integer not null,
    Cantidad integer not null,
    UnidadMedidaId integer not null,
    Precio decimal not null,
    ProveedorId integer not null,
    Activo number(1) not null
);

alter table Producto add (
    CONSTRAINT inventario_pk primary key(Id)
);

alter table Producto add constraint FK_Inventario_TipoProducto
    foreign key (TipoProductoId) references TipoProducto(Id);

alter table Producto add constraint FK_Inventario_UnidadMedida
    foreign key (UnidadMedidaId ) references UnidadMedida(Id);

alter table Producto add constraint FK_Inventario_Proveedor
    foreign key (ProveedorId) references Proveedor(Id);
    
Insert INTO Rol (Descripcion) VALUES ('Administrador');
Insert INTO Rol (Descripcion) VALUES ('Bodega');
Insert INTO Rol (Descripcion) VALUES ('Finanzas');
Insert INTO Rol (Descripcion) VALUES ('Cocina');
Insert INTO Rol (Descripcion) VALUES ('Garzon');
Insert INTO Rol (Descripcion) VALUES ('Cliente');

INSERT INTO Usuario (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion, Activo)
VALUES (1,'Segismundo', 'Morat', 'Segi_Mora168@SigloXXI.cl', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56922223760', 'Victoria,2251,Maipu', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion, Activo)
VALUES (2,'Laura', 'Paz', 'l_paz1999@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56967239551', 'Calama,6028,Pudahuel', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion, Activo)
VALUES (3,'Diego', 'Torres', 'diegox_ushiha@hotmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+569954670302', 'Jorge Hunneus,754,Quinta Normal', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion, Activo)
VALUES (4,'Valentina', 'Pincheira', 'valery-uwu@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56978902121', 'Gamero,529,Independencia', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion, Activo)
VALUES (4,'Henry', 'Zamora', 'dracula_darks@hotmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56934523033', 'Gamero,567,Independencia', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion, Activo)
VALUES (4,'Mario', 'Pino', 'mariop1993@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56923408485', 'San Martin,575,Santiago', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion, Activo)
VALUES (4,'Claudia', 'Salfate', 'salfate_clau94@outlook.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56943407675', 'Fermin Vivaceta,502,Independencia', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion, Activo)
VALUES (5,'Perla', 'San Martin', 'gema-ruby3@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56940331111', 'Recoleta,598,Recoleta', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion, Activo)
VALUES (5,'Alberto', 'Rivera', 'albersoto@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56925340555', 'Dominicana,200,Recoleta', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion, Activo)
VALUES (5,'Julia', 'Aries', 'juliana-kawaii@hotmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56934506653', 'Francisco Bilbao,6052,Providencia', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion, Activo)
VALUES (5,'Ares', 'Martines', 'fe4-jp@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56949492332', 'Gamero,531,Independencia', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion, Activo)
VALUES (5,'Eubanz', 'Brando', 'banz-87@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56922523042', 'Victoria,666,Maipu', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion, Activo)
VALUES (5,'Pablo', 'Mamani', 'perezpe3@hotmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56978924721', 'Duble almeyda,3755,Nuñoa', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion, Activo)
VALUES (6,'Maria', 'Bella', 'maripe@yopmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56975797876', '5 de abril,2220,Maipu', 0);
INSERT INTO Usuario (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion, Activo)
VALUES (6,'Carla', 'Brio', 'carlitax-5654@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56978221199', '3 Poniente, 2242, Maipu', 1);
INSERT INTO Usuario (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion, Activo)
VALUES (6,'Flor', 'Monsalva', 'for-mañanera@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56900004533', '4 Poniente 5044, Maipu', 1);

INSERT INTO Mesa (Numero, Descripcion, Capacidad, EnUso, Activa) VALUES (1,'mesa interior',2,0,1);
INSERT INTO Mesa (Numero, Descripcion, Capacidad, EnUso, Activa) VALUES (2,'mesa interior',4,0,1);
INSERT INTO Mesa (Numero, Descripcion, Capacidad, EnUso, Activa) VALUES (3,'mesa interior',4,0,1);
INSERT INTO Mesa (Numero, Descripcion, Capacidad, EnUso, Activa) VALUES (4,'mesa tv',6,0,1);
INSERT INTO Mesa (Numero, Descripcion, Capacidad, EnUso, Activa) VALUES (5,'barra bar',1,0,1);
INSERT INTO Mesa (Numero, Descripcion, Capacidad, EnUso, Activa) VALUES (6,'barra bar',1,0,1);
INSERT INTO Mesa (Numero, Descripcion, Capacidad, EnUso, Activa) VALUES (7,'mesa exterior',4,0,1);

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


COMMIT;