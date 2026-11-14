USE master;
GO

IF DB_ID(N'ControlEnviosCourierPractica3') IS NULL
BEGIN
    CREATE DATABASE ControlEnviosCourierPractica3;
END;
GO

USE ControlEnviosCourierPractica3;
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114233233_Inicial'
)
BEGIN
    CREATE TABLE [Clientes] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(100) NOT NULL,
        [Apellido] nvarchar(100) NOT NULL,
        [Cedula] nvarchar(25) NOT NULL,
        [Telefono] nvarchar(30) NOT NULL,
        [Correo] nvarchar(150) NOT NULL,
        [Direccion] nvarchar(250) NOT NULL,
        CONSTRAINT [PK_Clientes] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114233233_Inicial'
)
BEGIN
    CREATE TABLE [EstadosEnvio] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_EstadosEnvio] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114233233_Inicial'
)
BEGIN
    CREATE TABLE [Paquetes] (
        [Id] int NOT NULL IDENTITY,
        [Descripcion] nvarchar(250) NOT NULL,
        [Peso] decimal(10,2) NOT NULL,
        [FechaRegistro] datetime2 NOT NULL DEFAULT (SYSDATETIME()),
        [ClienteId] int NOT NULL,
        CONSTRAINT [PK_Paquetes] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_Paquetes_Peso] CHECK ([Peso] > 0),
        CONSTRAINT [FK_Paquetes_Clientes_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Clientes] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114233233_Inicial'
)
BEGIN
    CREATE TABLE [Envios] (
        [Id] int NOT NULL IDENTITY,
        [CodigoSeguimiento] nvarchar(50) NOT NULL,
        [PaqueteId] int NOT NULL,
        [EstadoEnvioId] int NOT NULL,
        [Tarifa] decimal(10,2) NOT NULL,
        [FechaEnvio] datetime2 NOT NULL,
        [FechaEntrega] datetime2 NULL,
        CONSTRAINT [PK_Envios] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_Envios_Fechas] CHECK ([FechaEntrega] IS NULL OR [FechaEntrega] >= [FechaEnvio]),
        CONSTRAINT [CK_Envios_Tarifa] CHECK ([Tarifa] >= 0),
        CONSTRAINT [FK_Envios_EstadosEnvio_EstadoEnvioId] FOREIGN KEY ([EstadoEnvioId]) REFERENCES [EstadosEnvio] ([Id]),
        CONSTRAINT [FK_Envios_Paquetes_PaqueteId] FOREIGN KEY ([PaqueteId]) REFERENCES [Paquetes] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114233233_Inicial'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Clientes_Cedula] ON [Clientes] ([Cedula]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114233233_Inicial'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Envios_CodigoSeguimiento] ON [Envios] ([CodigoSeguimiento]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114233233_Inicial'
)
BEGIN
    CREATE INDEX [IX_Envios_EstadoEnvioId] ON [Envios] ([EstadoEnvioId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114233233_Inicial'
)
BEGIN
    CREATE INDEX [IX_Envios_PaqueteId] ON [Envios] ([PaqueteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114233233_Inicial'
)
BEGIN
    CREATE UNIQUE INDEX [IX_EstadosEnvio_Nombre] ON [EstadosEnvio] ([Nombre]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114233233_Inicial'
)
BEGIN
    CREATE INDEX [IX_Paquetes_ClienteId] ON [Paquetes] ([ClienteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114233233_Inicial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261114233233_Inicial', N'10.0.0');
END;

COMMIT;
GO
