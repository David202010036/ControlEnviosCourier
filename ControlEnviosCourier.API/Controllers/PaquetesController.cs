using ControlEnviosCourier.Infrastructure.Repositories;
using ControlEnviosCourier.Domain.Entities;
using ControlEnviosCourier.API.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ControlEnviosCourier.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaquetesController : ControllerBase
    {
        private readonly PaqueteRepository repository;

        public PaquetesController(PaqueteRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult<List<PaqueteDto>> ObtenerPaquetes()
        {
            var paquetes = repository.GetAll();
            return Ok(paquetes.Select(ConvertirDto).ToList());
        }

        [HttpGet("{id:int}")]
        public ActionResult<PaqueteDto> ObtenerPaquete(int id)
        {
            var paquete = repository.GetById(id);

            if (paquete == null)
                return NotFound("Paquete no encontrado.");

            return Ok(ConvertirDto(paquete));
        }

        [HttpPost]
        public ActionResult<PaqueteDto> RegistrarPaquete(
            GuardarPaqueteDto datos)
        {
            if (!repository.ExisteCliente(datos.ClienteId))
                return BadRequest("El cliente indicado no existe.");

            var paquete = new Paquete
            {
                Descripcion = datos.Descripcion,
                Peso = datos.Peso,
                ClienteId = datos.ClienteId,
                FechaRegistro = DateTime.Now
            };

            repository.Create(paquete);

            return CreatedAtAction(
                nameof(ObtenerPaquete),
                new { id = paquete.Id },
                ConvertirDto(paquete));
        }

        [HttpPut("{id:int}")]
        public IActionResult ActualizarPaquete(
            int id, GuardarPaqueteDto datos)
        {
            var paquete = repository.GetById(id);

            if (paquete == null)
                return NotFound("Paquete no encontrado.");

            if (!repository.ExisteCliente(datos.ClienteId))
                return BadRequest("El cliente indicado no existe.");

            paquete.Descripcion = datos.Descripcion;
            paquete.Peso = datos.Peso;
            paquete.ClienteId = datos.ClienteId;

            repository.Update(paquete);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult EliminarPaquete(int id)
        {
            var paquete = repository.GetById(id);

            if (paquete == null)
                return NotFound("Paquete no encontrado.");

            if (repository.TieneEnvios(id))
                return Conflict("No se puede eliminar un paquete con envíos asociados.");

            repository.Delete(paquete);

            return NoContent();
        }

        private static PaqueteDto ConvertirDto(Paquete paquete)
        {
            return new PaqueteDto
            {
                Id = paquete.Id,
                Descripcion = paquete.Descripcion,
                Peso = paquete.Peso,
                FechaRegistro = DateTime.SpecifyKind(paquete.FechaRegistro, DateTimeKind.Unspecified),
                ClienteId = paquete.ClienteId
            };
        }
    }
}
