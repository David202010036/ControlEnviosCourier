USE master;
GO

IF DB_ID(N'ControlEnviosCourier') IS NULL
BEGIN
    CREATE DATABASE ControlEnviosCourier;
END;
GO

USE ControlEnviosCourier;
GO

IF OBJECT_ID(N'dbo.Clientes', N'U') IS NULL
BEGIN

    CREATE TABLE dbo.Clientes
    (
        Id INT IDENTITY(1,1) NOT NULL,
        Nombre NVARCHAR(100) NOT NULL,
        Apellido NVARCHAR(100) NOT NULL,
        Cedula NVARCHAR(25) NOT NULL,
        Telefono NVARCHAR(30) NOT NULL,
        Correo NVARCHAR(150) NOT NULL,
        Direccion NVARCHAR(250) NOT NULL,

        CONSTRAINT PK_Clientes PRIMARY KEY (Id),
        CONSTRAINT UQ_Clientes_Cedula UNIQUE (Cedula)
    );
END;
GO

IF OBJECT_ID(N'dbo.Paquetes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Paquetes
    (
        Id INT IDENTITY(1,1) NOT NULL,
        Descripcion NVARCHAR(250) NOT NULL,
        Peso DECIMAL(10,2) NOT NULL,
        FechaRegistro DATETIME2 NOT NULL
            CONSTRAINT DF_Paquetes_FechaRegistro
            DEFAULT (SYSDATETIME()),
        ClienteId INT NOT NULL,

        CONSTRAINT PK_Paquetes PRIMARY KEY (Id),
        CONSTRAINT CK_Paquetes_Peso CHECK (Peso > 0),
        CONSTRAINT FK_Paquetes_Clientes
            FOREIGN KEY (ClienteId)
            REFERENCES dbo.Clientes(Id)
    );
END;
GO

-- Comprobar que ambas tablas existen.
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = N'dbo'
  AND TABLE_TYPE = N'BASE TABLE'
  AND TABLE_NAME IN (N'Clientes', N'Paquetes');
GO