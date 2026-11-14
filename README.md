# Sistema de Control de Envíos para Courier

**Estudiante:** David Isaac Vásquez Somon

**Materia:** Programación II

**Tarea 4:** Capa de Servicios.

El proyecto permite registrar, consultar, actualizar y eliminar clientes, paquetes, envíos y estados de envío.

## Estructura

- **ControlEnviosCourier.API:** controladores y configuración de la conexión.
- **ControlEnviosCourier.Application:** DTO, servicios y ServiceResult.
- **ControlEnviosCourier.Domain:** entidades Cliente, Paquete, Envio y EstadoEnvio.
- **ControlEnviosCourier.Infrastructure:** contexto, migraciones y repositorios.

Los servicios validan los datos y realizan las operaciones mediante los repositorios. Los controladores consumen estos servicios.

## Ejecución

Abrir ControlEnviosCourier.slnx en Visual Studio. Configurar la conexión a SQL Server en appsettings.json y crear la base ControlEnviosCourierPractica4 desde SSMS con el archivo de la carpeta Database.

En SSMS, ejecutar primero la línea CREATE DATABASE y después el resto del archivo.

Ejecutar ControlEnviosCourier.API con el perfil https y abrir Swagger para utilizar los controladores.
