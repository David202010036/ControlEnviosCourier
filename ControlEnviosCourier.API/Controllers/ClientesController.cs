using ControlEnviosCourier.Infrastructure.Repositories;
using ControlEnviosCourier.Domain.Entities;
using ControlEnviosCourier.API.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ControlEnviosCourier.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteRepository repository;

        public ClientesController(ClienteRepository repository)
        {
            this.repository = repository;
        }

        // GET: api/Clientes
        [HttpGet]
        public ActionResult<List<ClienteDto>> ObtenerClientes()
        {
            var clientes = repository.GetAll();
            return Ok(clientes.Select(ConvertirDto).ToList());
        }

        // GET: api/Clientes/1
        [HttpGet("{id:int}")]
        public ActionResult<ClienteDto> ObtenerCliente(int id)
        {
            var cliente = repository.GetById(id);

            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            return Ok(ConvertirDto(cliente));
        }

        // POST: api/Clientes
        [HttpPost]
        public ActionResult<ClienteDto> RegistrarCliente(
            GuardarClienteDto datos)
        {
            if (repository.ExisteCedula(datos.Cedula))
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

            repository.Create(cliente);

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
            var cliente = repository.GetById(id);

            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            if (repository.ExisteCedula(datos.Cedula, id))
            {
                return Conflict("Ya existe otro cliente con esa cédula.");
            }

            cliente.Nombre = datos.Nombre;
            cliente.Apellido = datos.Apellido;
            cliente.Cedula = datos.Cedula;
            cliente.Telefono = datos.Telefono;
            cliente.Correo = datos.Correo;
            cliente.Direccion = datos.Direccion;

            repository.Update(cliente);

            return NoContent();
        }

        // DELETE: api/Clientes/1
        [HttpDelete("{id:int}")]
        public IActionResult EliminarCliente(int id)
        {
            var cliente = repository.GetById(id);

            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            if (repository.TienePaquetes(id))
            {
                return Conflict(
                    "No se puede eliminar un cliente con paquetes asociados.");
            }

            repository.Delete(cliente);

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
