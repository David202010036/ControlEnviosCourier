using ControlEnviosCourier.API.Data;
using ControlEnviosCourier.API.Models;
using ControlEnviosCourier.API.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ControlEnviosCourier.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ClientesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: api/Clientes
        [HttpGet]
        public ActionResult<List<ClienteDto>> ObtenerClientes()
        {
            var clientes = context.Clientes.ToList();
            return Ok(clientes.Select(ConvertirDto).ToList());
        }

        // GET: api/Clientes/1
        [HttpGet("{id:int}")]
        public ActionResult<ClienteDto> ObtenerCliente(int id)
        {
            var cliente = context.Clientes.Find(id);

            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            return Ok(ConvertirDto(cliente));
        }

        // POST: api/Clientes
        [HttpPost]
        public ActionResult<ClienteDto> RegistrarCliente(
            GuardarClienteDto datos)
        {
            if (context.Clientes.Any(c => c.Cedula == datos.Cedula))
                return Conflict("Ya existe un cliente con esa cédula.");

            var cliente = new Cliente
            {
                Nombre = datos.Nombre,
                Apellido = datos.Apellido,
                Cedula = datos.Cedula,
                Telefono = datos.Telefono,
                Correo = datos.Correo,
                Direccion = datos.Direccion
            };

            context.Clientes.Add(cliente);
            context.SaveChanges();

            return CreatedAtAction(
                nameof(ObtenerCliente),
                new { id = cliente.Id },
                ConvertirDto(cliente));
        }

        // PUT: api/Clientes/1
        [HttpPut("{id:int}")]
        public IActionResult ActualizarCliente(
            int id, GuardarClienteDto datos)
        {
            var cliente = context.Clientes.Find(id);

            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            if (context.Clientes.Any(
                c => c.Cedula == datos.Cedula && c.Id != id))
            {
                return Conflict("Ya existe otro cliente con esa cédula.");
            }

            cliente.Nombre = datos.Nombre;
            cliente.Apellido = datos.Apellido;
            cliente.Cedula = datos.Cedula;
            cliente.Telefono = datos.Telefono;
            cliente.Correo = datos.Correo;
            cliente.Direccion = datos.Direccion;

            context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Clientes/1
        [HttpDelete("{id:int}")]
        public IActionResult EliminarCliente(int id)
        {
            var cliente = context.Clientes.Find(id);

            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            if (context.Paquetes.Any(p => p.ClienteId == id))
            {
                return Conflict(
                    "No se puede eliminar un cliente con paquetes asociados.");
            }

            context.Clientes.Remove(cliente);
            context.SaveChanges();

            return NoContent();
        }

        private static ClienteDto ConvertirDto(Cliente cliente)
        {
            return new ClienteDto
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Cedula = cliente.Cedula,
                Telefono = cliente.Telefono,
                Correo = cliente.Correo,
                Direccion = cliente.Direccion
            };
        }
    }
}