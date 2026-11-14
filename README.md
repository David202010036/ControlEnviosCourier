# Control de Envíos para Courier

**Autor:** David Isaac Vásquez Somon

**Asignatura:** Programación II

**Práctica 2:** CRUD Web API en ASP.NET Core con SQL Server.

Esta entrega implementa la administración de clientes y paquetes. Cada paquete pertenece a un cliente. Los datos se conservan en SQL Server al reiniciar la API.

## Tecnologías

- C# y ASP.NET Core Web API con .NET 10.
- Controllers y DTO para solicitudes y respuestas.
- Entity Framework Core y SQL Server.
- Swagger para consultar y probar los endpoints.

## Requisitos

- SDK de .NET 10.
- SQL Server; se comprobó con SQL Server 2022 Express.
- SQL Server Management Studio (SSMS) o `sqlcmd` para ejecutar el script SQL.
- Visual Studio con la carga de trabajo de desarrollo de ASP.NET y web, o la CLI de .NET.

## Instalación y ejecución

### 1. Descargar el repositorio

```powershell
git clone https://github.com/David202010036/ControlEnviosCourier.git
cd ControlEnviosCourier
```

### 2. Crear la base de datos

En SSMS, conectarse al servidor y ejecutar `Database/CrearBaseDeDatos.sql`.

El script crea la base `ControlEnviosCourier`, las tablas `Clientes` y `Paquetes`, la restricción de cédula única y la relación mediante `ClienteId`. Si la base o las tablas ya existen, no las vuelve a crear ni modifica su estructura.

También puede ejecutarse desde PowerShell, con autenticación de Windows:

```powershell
sqlcmd -S '.\SQLEXPRESS' -E -b -i Database/CrearBaseDeDatos.sql
```

### 3. Configurar la conexión

En `ControlEnviosCourier.API/appsettings.json`, ajustar el servidor a la instancia local:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=ControlEnviosCourier;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

La configuración utiliza el usuario de Windows que ejecuta la API. Ese usuario debe tener acceso a la base de datos. `TrustServerCertificate=True` se utiliza para el entorno local de desarrollo.

El contexto se configura manualmente para las tablas existentes. Esta entrega utiliza el script SQL para crear la base; no requiere ejecutar migraciones.

### 4. Restaurar y ejecutar

Desde la raíz del repositorio:

```powershell
dotnet restore ControlEnviosCourier.API/ControlEnviosCourier.API.csproj
dotnet build ControlEnviosCourier.API/ControlEnviosCourier.API.csproj
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
  Data/
    ApplicationDbContext.cs
  Models/
    Cliente.cs
    Paquete.cs
    Dtos/
      ClienteDto.cs
      GuardarClienteDto.cs
      PaqueteDto.cs
      GuardarPaqueteDto.cs
  Properties/
    launchSettings.json
  appsettings.json
  Program.cs
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

## Validaciones y errores

- Campos de texto obligatorios y longitudes máximas según los DTO.
- Correo electrónico con formato válido.
- Peso entre 0.01 y 99999999.99.
- ClienteId positivo y correspondiente a un cliente existente.
- Cédula única por cliente.
- Un cliente con paquetes asociados no puede eliminarse: primero se deben eliminar o reasignar sus paquetes.

| Código | Situación |
| --- | --- |
| 400 | Datos inválidos o cliente asociado inexistente |
| 404 | Cliente o paquete solicitado inexistente |
| 409 | Cédula duplicada o intento de eliminar un cliente con paquetes |

## Verificación realizada

Se realizaron 39 comprobaciones HTTP que cubrieron registro, consulta, actualización, eliminación, validaciones, relaciones y conservación de los datos después de reiniciar la API. Los registros ficticios usados en esas pruebas se eliminaron al finalizar.

También se verificó el script en una base temporal nueva: creó ambas tablas y la clave foránea correctamente. La revisión final de compilación terminó con cero errores y cero advertencias, y la auditoría NuGet no reportó vulnerabilidades conocidas en los paquetes resueltos.

Para repetir la comprobación funcional, ejecutar POST y GET de ambas entidades, actualizar mediante PUT y consultar los cambios después de reiniciar la API. Al terminar, eliminar primero el paquete y luego el cliente. Comprobar también las respuestas 400, 404 y 409 descritas anteriormente.

## Alcance de esta entrega

La práctica incluye los CRUD de Clientes y Paquetes. La gestión completa de envíos, tarifas, entregas y seguimiento corresponde a la continuación del proyecto final.
