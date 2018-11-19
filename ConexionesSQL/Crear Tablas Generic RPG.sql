CREATE TABLE estado_usuario(
id_estado int not null,
descripcion varchar(50) not null,
PRIMARY KEY (id_estado)
);

CREATE TABLE tipo_usuario(
id_tipo int not null,
descripcion varchar(50) not null,
PRIMARY key (id_tipo)
);

CREATE TABLE usuario(
nickname varchar(50) not null,
nombres varchar(30) not null,
apellidos varchar(30) not null,
correo varchar(100) unique not null,
hash varchar(100) not null,
salt varchar(50) not null,
fecha_nacimiento date not null,
fecha_creacion date not null,
id_tipo int not null ,
id_estado int not null,
PRIMARY KEY (nickname),
FOREIGN KEY (id_estado) references estado_usuario(id_estado), 
FOREIGN KEY (id_tipo) references tipo_usuario(id_tipo)
);

CREATE TABLE archivo_guardado(
id_archivo int not null,
archivo BLOB not null,
fecha_guardado date not null,
nickname varchar(50) not null,
PRIMARY KEY(id_archivo),
FOREIGN KEY(nickname) references usuario(nickname)
);

CREATE TABLE historial_conexion(
nickname varchar(50) not null,
fecha_conexion datetime not null);

alter table historial_conexion
	add constraint historial_nick foreign key (nickname) references usuario(nickname);
alter table historial_conexion
	add constraint historial_pk primary key (nickname, fecha_conexion);

Create table usuario_con(
id_cuenta int AUTO_INCREMENT not null, 
nickname varchar(50) not null, 
conexiones int not null,
PRIMARY KEY (id_cuenta),
FOREIGN KEY(nickname) references usuario (nickname)
);

DELIMITER $$
Create trigger tr_conexiones AFTER INSERT ON historial_conexion
	FOR EACH ROW
BEGIN
	Update usuario_con
	Set conexiones = conexiones+1
	Where nickname = NEW.nickname;
END$$
DELIMITER ;

DELIMITER $$
Create trigger tr_usuarios
	AFTER INSERT ON usuario
	FOR EACH ROW
BEGIN
	insert into usuario_con(nickname, conexiones) values(NEW.nickname, 0);
End$$
DELIMITER ;

INSERT INTO `estado_usuario` (`id_estado`, `descripcion`) VALUES ('1', 'Activo');
INSERT INTO `estado_usuario` (`id_estado`, `descripcion`) VALUES ('2', 'Bloqueado');
INSERT INTO `tipo_usuario` (`id_tipo`, `descripcion`) VALUES ('1', 'Administrador');
INSERT INTO `tipo_usuario` (`id_tipo`, `descripcion`) VALUES ('2', 'Usuario');