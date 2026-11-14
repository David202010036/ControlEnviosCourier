using ControlEnviosCourier.API.Models.Dtos;
using ControlEnviosCourier.Domain.Entities;
using ControlEnviosCourier.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ControlEnviosCourier.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadosEnvioController : ControllerBase
    {
        private readonly GenericRepository<EstadoEnvio> repository;
        private readonly GenericRepository<Envio> envios;

        public EstadosEnvioController(GenericRepository<EstadoEnvio> repository,
            GenericRepository<Envio> envios)
        {
            this.repository = repository;
            this.envios = envios;
        }

        [HttpGet]
        public ActionResult<List<EstadoEnvioDto>> ObtenerEstados()
        {
            return Ok(repository.GetAll().Select(ConvertirDto).ToList());
        }

        [HttpGet("{id:int}")]
        public ActionResult<EstadoEnvioDto> ObtenerEstado(int id)
        {
            var estado = repository.GetById(id);
            if (estado == null) return NotFound("Estado no encontrado.");
            return Ok(ConvertirDto(estado));
        }

        [HttpPost]
        public ActionResult<EstadoEnvioDto> RegistrarEstado(GuardarEstadoEnvioDto datos)
        {
            if (repository.Any(e => e.Nombre == datos.Nombre))
                return Conflict("Ya existe un estado con ese nombre.");
            var estado = repository.Create(new EstadoEnvio { Nombre = datos.Nombre });
            return CreatedAtAction(nameof(ObtenerEstado), new { id = estado.Id }, ConvertirDto(estado));
        }

        [HttpPut("{id:int}")]
        public IActionResult ActualizarEstado(int id, GuardarEstadoEnvioDto datos)
        {
            var estado = repository.GetById(id);
            if (estado == null) return NotFound("Estado no encontrado.");
            if (repository.Any(e => e.Nombre == datos.Nombre && e.Id != id))
                return Conflict("Ya existe otro estado con ese nombre.");
            estado.Nombre = datos.Nombre;
            repository.Update(estado);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult EliminarEstado(int id)
        {
            var estado = repository.GetById(id);
            if (estado == null) return NotFound("Estado no encontrado.");
            if (envios.Any(e => e.EstadoEnvioId == id))
                return Conflict("No se puede eliminar un estado utilizado por envíos.");
            repository.Delete(estado);
            return NoContent();
        }

        private static EstadoEnvioDto ConvertirDto(EstadoEnvio estado)
        {
            return new EstadoEnvioDto { Id = estado.Id, Nombre = estado.Nombre };
        }
    }
}
