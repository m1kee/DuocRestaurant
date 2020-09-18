-- script de creación de schema de la bd de duoc restaurant

create table ROL(
    Id integer generated always as identity (start with 1 increment by 1),
    Rol nvarchar2(20) not null
);

alter table ROL add(
    CONSTRAINT rol_pk primary key(Id)
);

create table USUARIO(
    Id integer generated always as identity (start with 1 increment by 1),
    RolId integer not null,
    Nombre nvarchar2(30) not null,
    Apellido nvarchar2(30) not null,
    Correo nvarchar2(100) not null,
    Contrasena nvarchar2(64) not null,
    Telefono nvarchar2(15) not null,
    Direccion nvarchar2(200)
);

alter table USUARIO add(
    CONSTRAINT usuario_pk primary key(Id)
);

alter table USUARIO add constraint roll_fk
    foreign key (RolId) references ROL(Id)
    DEFERRABLE initially deferred;
    
create table EMPLEADO(
    Id integer generated always as identity (start with 1 increment by 1),
    UsuarioId integer not null,
    FechaContrato date not null,
    Salario integer not null
);

alter table EMPLEADO add(
    CONSTRAINT empleado_pk primary key(Id)
);

alter table EMPLEADO add constraint usuario_fk
    foreign key (UsuarioId) references Usuario(Id)
    DEFERRABLE initially deferred;

create table MESA(
    Id integer generated always as identity (start with 1 increment by 1),
    Numero integer not null,
    Descripcion varchar(30),
    Capacidad integer,
    Activa number(1) not null,
    EnUso number(1) not null
);

alter table MESA add(
    CONSTRAINT mesa_pk primary key(Id)
);
    
Insert into ROL (Rol) Values ('Administrador');
Insert into ROL (Rol) Values ('Bodega');
Insert into ROL (Rol) Values ('Finanzas');
Insert into ROL (Rol) Values ('Cocina');
Insert into ROL (Rol) Values ('Garzon');
Insert into ROL (Rol) Values ('Cliente');

insert into USUARIO (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion)
Values(1,'Segismundo', 'Morat', 'Segi_Mora168@SigloXXI.cl', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56922223760', 'Victoria,2251,Maipu');
insert into USUARIO (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion)
Values(2,'Laura', 'Paz', 'l_paz1999@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56967239551', 'Calama,6028,Pudahuel');
insert into USUARIO (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion)
Values(3,'Diego', 'Torres', 'diegox_ushiha@hotmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+569954670302', 'Jorge Hunneus,754,Quinta Normal');
insert into USUARIO (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion)
Values(4,'Valentina', 'Pincheira', 'valery-uwu@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56978902121', 'Gamero,529,Independencia');
insert into USUARIO (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion)
Values(4,'Henry', 'Zamora', 'dracula_darks@hotmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56934523033', 'Gamero,567,Independencia');
insert into USUARIO (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion)
Values(4,'Mario', 'Pino', 'mariop1993@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56923408485', 'San Martin,575,Santiago');
insert into USUARIO (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion)
Values(4,'Claudia', 'Salfate', 'salfate_clau94@outlook.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56943407675', 'Fermin Vivaceta,502,Independencia');
insert into USUARIO (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion)
Values(5,'Perla', 'San Martin', 'gema-ruby3@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56940331111', 'Recoleta,598,Recoleta');
insert into USUARIO (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion)
Values(5,'Alberto', 'Rivera', 'albersoto@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56925340555', 'Dominicana,200,Recoleta');
insert into USUARIO (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion)
Values(5,'Julia', 'Aries', 'juliana-kawaii@hotmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56934506653', 'Francisco Bilbao,6052,Providencia');
insert into USUARIO (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion)
Values(5,'Ares', 'Martines', 'fe4-jp@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56949492332', 'Gamero,531,Independencia');
insert into USUARIO (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion)
Values(5,'Eubanz', 'Brando', 'banz-87@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56922523042', 'Victoria,666,Maipu');
insert into USUARIO (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion)
Values(5,'Pablo', 'Mamani', 'perezpe3@hotmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56978924721', 'Duble almeyda,3755,Nuñoa');
insert into USUARIO (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion)
Values(6,'Maria', 'Bella', 'maripe@yopmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56975797876', '5 de abril,2220,Maipu');
insert into USUARIO (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion)
Values(6,'Carla', 'Brio', 'carlitax-5654@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56978221199', '3 Poniente, 2242, Maipu');
insert into USUARIO (RolId, Nombre, Apellido, Correo, Contrasena, Telefono, Direccion)
Values(6,'Flor', 'Monsalva', 'for-mañanera@gmail.com', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', '+56900004533', '4 Poniente 5044, Maipu');

insert into EMPLEADO (UsuarioID,FechaContrato,Salario) values (1, to_date('05/10/1970', 'DD/MM/YYYY'),2500000);
insert into EMPLEADO (UsuarioID,FechaContrato,Salario) values (2, to_date('02/04/2002', 'DD/MM/YYYY'),700000);
insert into EMPLEADO (UsuarioID,FechaContrato,Salario) values (3, to_date('11/12/1987', 'DD/MM/YYYY'),680000);
insert into EMPLEADO (UsuarioID,FechaContrato,Salario) values (4, to_date('03/11/2010', 'DD/MM/YYYY'),540000);
insert into EMPLEADO (UsuarioID,FechaContrato,Salario) values (5, to_date('15/02/2012', 'DD/MM/YYYY'),320000);
insert into EMPLEADO (UsuarioID,FechaContrato,Salario) values (6, to_date('25/12/2012', 'DD/MM/YYYY'),320000);
insert into EMPLEADO (UsuarioID,FechaContrato,Salario) values (7, to_date('20/01/2000', 'DD/MM/YYYY'),600000);
insert into EMPLEADO (UsuarioID,FechaContrato,Salario) values (8, to_date('07/08/2015', 'DD/MM/YYYY'),210000);
insert into EMPLEADO (UsuarioID,FechaContrato,Salario) values (9, to_date('18/03/2020', 'DD/MM/YYYY'),170000);
insert into EMPLEADO (UsuarioID,FechaContrato,Salario) values (10,to_date('04/10/2019', 'DD/MM/YYYY'),190000);
insert into EMPLEADO (UsuarioID,FechaContrato,Salario) values (11,to_date('14/05/2014', 'DD/MM/YYYY'),210000);
insert into EMPLEADO (UsuarioID,FechaContrato,Salario) values (12,to_date('30/04/2021', 'DD/MM/YYYY'),190000);
insert into EMPLEADO (UsuarioID,FechaContrato,Salario) values (13,to_date('10/08/2021', 'DD/MM/YYYY'),190000);

insert into MESA (Numero, Descripcion, Capacidad, EnUso, Activa) values (1,'mesa interior',2,0,1);
insert into MESA (Numero, Descripcion, Capacidad, EnUso, Activa) values (2,'mesa interior',2,0,1);
insert into MESA (Numero, Descripcion, Capacidad, EnUso, Activa) values (3,'mesa interior',2,0,1);
insert into MESA (Numero, Descripcion, Capacidad, EnUso, Activa) values (4,'mesa interior',4,0,1);
insert into MESA (Numero, Descripcion, Capacidad, EnUso, Activa) values (5,'mesa interior',4,0,1);
insert into MESA (Numero, Descripcion, Capacidad, EnUso, Activa) values (6,'mesa tv',6,0,1);
insert into MESA (Numero, Descripcion, Capacidad, EnUso, Activa) values (7,'mesa tv',6,0,1);
insert into MESA (Numero, Descripcion, Capacidad, EnUso, Activa) values (8,'barra bar',1,0,1);
insert into MESA (Numero, Descripcion, Capacidad, EnUso, Activa) values (9,'barra bar',1,0,1);
insert into MESA (Numero, Descripcion, Capacidad, EnUso, Activa) values (10,'barra bar',1,0,1);
insert into MESA (Numero, Descripcion, Capacidad, EnUso, Activa) values (11,'barra bar',1,0,1);
insert into MESA (Numero, Descripcion, Capacidad, EnUso, Activa) values (12,'barra bar',1,0,1);
insert into MESA (Numero, Descripcion, Capacidad, EnUso, Activa) values (13,'mesa exterior',4,0,1);
insert into MESA (Numero, Descripcion, Capacidad, EnUso, Activa) values (14,'mesa exterior',4,0,1);
insert into MESA (Numero, Descripcion, Capacidad, EnUso, Activa) values (15,'mesa exterior',2,0,1);
