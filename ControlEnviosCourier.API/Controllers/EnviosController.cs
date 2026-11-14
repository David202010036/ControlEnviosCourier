using ControlEnviosCourier.API.Models.Dtos;
using ControlEnviosCourier.Domain.Entities;
using ControlEnviosCourier.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ControlEnviosCourier.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnviosController : ControllerBase
    {
        private readonly GenericRepository<Envio> repository;
        private readonly PaqueteRepository paquetes;
        private readonly GenericRepository<EstadoEnvio> estados;

        public EnviosController(GenericRepository<Envio> repository,
            PaqueteRepository paquetes, GenericRepository<EstadoEnvio> estados)
        {
            this.repository = repository;
            this.paquetes = paquetes;
            this.estados = estados;
        }

        [HttpGet]
        public ActionResult<List<EnvioDto>> ObtenerEnvios()
        {
            return Ok(repository.GetAll().Select(ConvertirDto).ToList());
        }

        [HttpGet("{id:int}")]
        public ActionResult<EnvioDto> ObtenerEnvio(int id)
        {
            var envio = repository.GetById(id);
            if (envio == null) return NotFound("Envío no encontrado.");
            return Ok(ConvertirDto(envio));
        }

        [HttpPost]
        public ActionResult<EnvioDto> RegistrarEnvio(GuardarEnvioDto datos)
        {
            if (paquetes.GetById(datos.PaqueteId) == null)
                return BadRequest("El paquete indicado no existe.");
            if (estados.GetById(datos.EstadoEnvioId) == null)
                return BadRequest("El estado indicado no existe.");
            var envio = repository.Create(new Envio
            {
                CodigoSeguimiento = Guid.NewGuid().ToString("N"),
                PaqueteId = datos.PaqueteId,
                EstadoEnvioId = datos.EstadoEnvioId,
                Tarifa = datos.Tarifa,
                FechaEnvio = datos.FechaEnvio,
                FechaEntrega = datos.FechaEntrega
            });
            return CreatedAtAction(nameof(ObtenerEnvio), new { id = envio.Id }, ConvertirDto(envio));
        }

        [HttpPut("{id:int}")]
        public IActionResult ActualizarEnvio(int id, GuardarEnvioDto datos)
        {
            var envio = repository.GetById(id);
            if (envio == null) return NotFound("Envío no encontrado.");
            if (paquetes.GetById(datos.PaqueteId) == null)
                return BadRequest("El paquete indicado no existe.");
            if (estados.GetById(datos.EstadoEnvioId) == null)
                return BadRequest("El estado indicado no existe.");
            envio.PaqueteId = datos.PaqueteId;
            envio.EstadoEnvioId = datos.EstadoEnvioId;
            envio.Tarifa = datos.Tarifa;
            envio.FechaEnvio = datos.FechaEnvio;
            envio.FechaEntrega = datos.FechaEntrega;
            repository.Update(envio);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult EliminarEnvio(int id)
        {
            var envio = repository.GetById(id);
            if (envio == null) return NotFound("Envío no encontrado.");
            repository.Delete(envio);
            return NoContent();
        }

        private static EnvioDto ConvertirDto(Envio envio)
        {
            return new EnvioDto
            {
                Id = envio.Id,
                CodigoSeguimiento = envio.CodigoSeguimiento,
                PaqueteId = envio.PaqueteId,
                EstadoEnvioId = envio.EstadoEnvioId,
                Tarifa = envio.Tarifa,
                FechaEnvio = DateTime.SpecifyKind(envio.FechaEnvio, DateTimeKind.Unspecified),
                FechaEntrega = envio.FechaEntrega.HasValue
                    ? DateTime.SpecifyKind(envio.FechaEntrega.Value, DateTimeKind.Unspecified)
                    : null
            };
        }
    }
}
