# Sistema de Control de Envíos para Courier

**Estudiante:** David Isaac Vásquez Somon

**Materia:** Programación II

**Tarea 3:** Capa de Dominio e Infraestructura.

El proyecto permite registrar, consultar, actualizar y eliminar clientes, paquetes, envíos y estados de envío.

## Estructura

- **ControlEnviosCourier.API:** controladores, DTO y configuración de la conexión.
- **ControlEnviosCourier.Domain:** entidades Cliente, Paquete, Envio y EstadoEnvio.
- **ControlEnviosCourier.Infrastructure:** contexto, migraciones y repositorios.

Se utilizan ClienteRepository y PaqueteRepository como repositorios específicos y GenericRepository para Envio y EstadoEnvio. Los controladores trabajan mediante los repositorios.

## Ejecución

Abrir ControlEnviosCourier.slnx en Visual Studio. Configurar la conexión a SQL Server en appsettings.json y crear la base ControlEnviosCourierPractica3 desde SSMS con el archivo de la carpeta Database.

En SSMS, ejecutar primero la línea CREATE DATABASE y después el resto del archivo.

Ejecutar ControlEnviosCourier.API con el perfil https y abrir Swagger para utilizar los controladores.
