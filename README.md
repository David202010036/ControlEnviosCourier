# Control de Envíos para Courier

**Autor:** David Isaac Vásquez Somon

**Asignatura:** Programación II

**Práctica 3:** Capa de Dominio e Infraestructura.

API para registrar, consultar, actualizar y eliminar clientes, paquetes, estados y envíos de un courier. Los datos se guardan en SQL Server.

La solución contiene tres proyectos: API, Domain e Infrastructure. Los controladores reciben repositorios por inyección de dependencias y no acceden directamente al contexto. ClienteRepository y PaqueteRepository heredan de GenericRepository; Envíos y Estados utilizan el repositorio genérico.

## Tecnologías

- C# y ASP.NET Core Web API con .NET 10.
- Controllers y DTO para solicitudes y respuestas.
- Entity Framework Core y SQL Server.
- Swagger para consultar y probar los endpoints.

## Requisitos

- SDK de .NET 10.
- SQL Server 2022 Express.
- SQL Server Management Studio (SSMS) o `sqlcmd` para ejecutar el script SQL.
- Visual Studio con la carga de trabajo de desarrollo de ASP.NET y web, o la CLI de .NET.

## Instalación y ejecución

### 1. Descargar el repositorio

```powershell
git clone --branch tarea-3 https://github.com/David202010036/ControlEnviosCourier.git
cd ControlEnviosCourier
```

### 2. Crear la base de datos

Esta rama utiliza la base **ControlEnviosCourierPractica3**, independiente de la base de la práctica 2.

Las migraciones están en Infrastructure. Desde la raíz del repositorio se puede crear la base con:

```powershell
dotnet restore ControlEnviosCourier.slnx
dotnet tool restore
dotnet ef database update --project ControlEnviosCourier.Infrastructure --startup-project ControlEnviosCourier.API
```

La migración inicial crea Clientes, Paquetes, EstadosEnvio y Envios, con sus relaciones y restricciones.

Como alternativa, en SSMS ejecutar `Database/CrearBaseDeDatos.sql`. El script crea la base y aplica la migración si todavía no está registrada.

También puede ejecutarse desde PowerShell, con autenticación de Windows:

```powershell
sqlcmd -S '.\SQLEXPRESS' -E -b -i Database/CrearBaseDeDatos.sql
```

### 3. Configurar la conexión

En `ControlEnviosCourier.API/appsettings.json`, ajustar el servidor a la instancia local:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=ControlEnviosCourierPractica3;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

La configuración utiliza el usuario de Windows que ejecuta la API. Ese usuario debe tener acceso a la base de datos. `TrustServerCertificate=True` se utiliza para el entorno local de desarrollo.

Si se cambia el nombre de la base, debe coincidir en la conexión de la API y en el script SQL. No es necesario ejecutar el script si ya se creó la base con las migraciones.

### 4. Restaurar y ejecutar

Desde la raíz del repositorio:

```powershell
dotnet restore ControlEnviosCourier.slnx
dotnet build ControlEnviosCourier.slnx
dotnet dev-certs https --trust
dotnet run --project ControlEnviosCourier.API/ControlEnviosCourier.API.csproj --launch-profile https
```

Abrir **https://localhost:7144/swagger/index.html**. Swagger está habilitado en el entorno `Development`, configurado en el perfil `https`.

En Visual Studio, abrir `ControlEnviosCourier.slnx`, seleccionar el proyecto API como proyecto de inicio y ejecutar el perfil `https`. El navegador se abre manualmente.

## Estructura

```text
ControlEnviosCourier.slnx
Database/
  CrearBaseDeDatos.sql
ControlEnviosCourier.API/
  Controllers/
    ClientesController.cs
    PaquetesController.cs
    EstadosEnvioController.cs
    EnviosController.cs
  Models/
    Dtos/
      ClienteDto.cs
      GuardarClienteDto.cs
      PaqueteDto.cs
      GuardarPaqueteDto.cs
      EstadoEnvioDto.cs
      GuardarEstadoEnvioDto.cs
      EnvioDto.cs
      GuardarEnvioDto.cs
  Properties/
    launchSettings.json
  appsettings.json
  Program.cs
ControlEnviosCourier.Domain/
  Core/
    BaseEntity.cs
  Entities/
    Cliente.cs
    Paquete.cs
    EstadoEnvio.cs
    Envio.cs
ControlEnviosCourier.Infrastructure/
  Context/
    ApplicationDbContext.cs
  Migrations/
  Repositories/
    GenericRepository.cs
    ClienteRepository.cs
    PaqueteRepository.cs
```

## Endpoints

| Método | Ruta | Operación | Respuesta correcta |
| --- | --- | --- | --- |
| GET | `/api/Clientes` | Listar clientes | 200 |
| GET | `/api/Clientes/{id}` | Consultar un cliente | 200 |
| POST | `/api/Clientes` | Registrar un cliente | 201 |
| PUT | `/api/Clientes/{id}` | Actualizar un cliente | 204 |
| DELETE | `/api/Clientes/{id}` | Eliminar un cliente | 204 |
| GET | `/api/Paquetes` | Listar paquetes | 200 |
| GET | `/api/Paquetes/{id}` | Consultar un paquete | 200 |
| POST | `/api/Paquetes` | Registrar un paquete | 201 |
| PUT | `/api/Paquetes/{id}` | Actualizar un paquete | 204 |
| DELETE | `/api/Paquetes/{id}` | Eliminar un paquete | 204 |
| GET | `/api/EstadosEnvio` | Listar estados | 200 |
| GET | `/api/EstadosEnvio/{id}` | Consultar un estado | 200 |
| POST | `/api/EstadosEnvio` | Registrar un estado | 201 |
| PUT | `/api/EstadosEnvio/{id}` | Actualizar un estado | 204 |
| DELETE | `/api/EstadosEnvio/{id}` | Eliminar un estado | 204 |
| GET | `/api/Envios` | Listar envíos | 200 |
| GET | `/api/Envios/{id}` | Consultar un envío | 200 |
| POST | `/api/Envios` | Registrar un envío | 201 |
| PUT | `/api/Envios/{id}` | Actualizar un envío | 204 |
| DELETE | `/api/Envios/{id}` | Eliminar un envío | 204 |

POST asigna el Id automáticamente y devuelve el recurso creado y su ubicación. PUT recibe todos los campos editables. Las respuestas 204 no contienen cuerpo.

## Ejemplos en Swagger

Registrar primero un cliente con POST `/api/Clientes`:

```json
{
  "nombre": "Ana",
  "apellido": "Perez",
  "cedula": "001-0000000-1",
  "telefono": "+1 809-555-0100",
  "correo": "ana@example.com",
  "direccion": "Calle de prueba 123"
}
```

Registrar un paquete con POST `/api/Paquetes`, reemplazando `clienteId` por el Id que devolvió la operación anterior:

```json
{
  "descripcion": "Caja de libros",
  "peso": 2.5,
  "clienteId": 1
}
```

El peso se expresa en kilogramos y se almacena con dos decimales. La fecha de registro la asigna la API al crear el paquete y no se modifica mediante PUT.

Crear un estado con POST `/api/EstadosEnvio`:

```json
{
  "nombre": "Pendiente"
}
```

Después crear un envío con POST `/api/Envios`, usando los Id reales del paquete y del estado:

```json
{
  "paqueteId": 1,
  "estadoEnvioId": 1,
  "tarifa": 150.00,
  "fechaEnvio": "2026-11-14T10:00:00",
  "fechaEntrega": null
}
```

El código de seguimiento se genera al registrar el envío y se conserva al actualizarlo. Las fechas se manejan como fecha y hora local, sin zona horaria. La tarifa es el importe registrado para ese envío y se almacena con dos decimales.

## Validaciones y errores

- Campos de texto obligatorios y longitudes máximas según los DTO.
- Correo electrónico con formato válido.
- Peso entre 0.01 y 99999999.99.
- ClienteId positivo y correspondiente a un cliente existente.
- Cédula única por cliente.
- Nombre de estado único.
- Paquete y estado existentes al registrar o actualizar un envío.
- Tarifa no negativa y fecha de entrega igual o posterior a la fecha de envío.
- Un cliente con paquetes asociados no puede eliminarse: primero se deben eliminar o reasignar sus paquetes.
- Un paquete con envíos o un estado utilizado por envíos tampoco pueden eliminarse. Para borrar registros relacionados, comenzar por el envío.

| Código | Situación |
| --- | --- |
| 400 | Datos inválidos o recurso asociado inexistente |
| 404 | Recurso solicitado inexistente |
| 409 | Cédula o nombre de estado duplicado, o relaciones que impiden eliminar |

## Entrega

- Práctica 3: https://github.com/David202010036/ControlEnviosCourier/tree/tarea-3
- El mismo contenido se publica en develop.
- La entrega de la práctica 2 se conserva en main.
