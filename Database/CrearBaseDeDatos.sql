CREATE DATABASE ControlEnviosCourierPractica3;

USE ControlEnviosCourierPractica3;

CREATE TABLE Clientes
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Cedula NVARCHAR(25) NOT NULL UNIQUE,
    Telefono NVARCHAR(30) NOT NULL,
    Correo NVARCHAR(150) NOT NULL,
    Direccion NVARCHAR(250) NOT NULL
);

CREATE TABLE EstadosEnvio
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Paquetes
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(250) NOT NULL,
    Peso DECIMAL(10,2) NOT NULL CHECK (Peso > 0),
    FechaRegistro DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    ClienteId INT NOT NULL,
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id)
);

CREATE TABLE Envios
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CodigoSeguimiento NVARCHAR(50) NOT NULL UNIQUE,
    PaqueteId INT NOT NULL,
    EstadoEnvioId INT NOT NULL,
    Tarifa DECIMAL(10,2) NOT NULL CHECK (Tarifa >= 0),
    FechaEnvio DATETIME2 NOT NULL,
    FechaEntrega DATETIME2 NULL,
    CHECK (FechaEntrega IS NULL OR FechaEntrega >= FechaEnvio),
    FOREIGN KEY (PaqueteId) REFERENCES Paquetes(Id),
    FOREIGN KEY (EstadoEnvioId) REFERENCES EstadosEnvio(Id)
);
