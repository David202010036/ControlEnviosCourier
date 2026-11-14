using ControlEnviosCourier.API.Data;
using ControlEnviosCourier.API.Models;
using ControlEnviosCourier.API.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ControlEnviosCourier.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaquetesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public PaquetesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<List<PaqueteDto>> ObtenerPaquetes()
        {
            var paquetes = context.Paquetes.ToList();
            return Ok(paquetes.Select(ConvertirDto).ToList());
        }

        [HttpGet("{id:int}")]
        public ActionResult<PaqueteDto> ObtenerPaquete(int id)
        {
            var paquete = context.Paquetes.Find(id);

            if (paquete == null)
                return NotFound("Paquete no encontrado.");

            return Ok(ConvertirDto(paquete));
        }

        [HttpPost]
        public ActionResult<PaqueteDto> RegistrarPaquete(
            GuardarPaqueteDto datos)
        {
            if (!context.Clientes.Any(c => c.Id == datos.ClienteId))
                return BadRequest("El cliente indicado no existe.");

            var paquete = new Paquete
            {
                Descripcion = datos.Descripcion,
                Peso = datos.Peso,
                ClienteId = datos.ClienteId,
                FechaRegistro = DateTime.Now
            };

            context.Paquetes.Add(paquete);
            context.SaveChanges();

            return CreatedAtAction(
                nameof(ObtenerPaquete),
                new { id = paquete.Id },
                ConvertirDto(paquete));
        }

        [HttpPut("{id:int}")]
        public IActionResult ActualizarPaquete(
            int id, GuardarPaqueteDto datos)
        {
            var paquete = context.Paquetes.Find(id);

            if (paquete == null)
                return NotFound("Paquete no encontrado.");

            if (!context.Clientes.Any(c => c.Id == datos.ClienteId))
                return BadRequest("El cliente indicado no existe.");

            paquete.Descripcion = datos.Descripcion;
            paquete.Peso = datos.Peso;
            paquete.ClienteId = datos.ClienteId;

            context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult EliminarPaquete(int id)
        {
            var paquete = context.Paquetes.Find(id);

            if (paquete == null)
                return NotFound("Paquete no encontrado.");

            context.Paquetes.Remove(paquete);
            context.SaveChanges();

            return NoContent();
        }

        private static PaqueteDto ConvertirDto(Paquete paquete)
        {
            return new PaqueteDto
            {
                Id = paquete.Id,
                Descripcion = paquete.Descripcion,
                Peso = paquete.Peso,
                FechaRegistro = paquete.FechaRegistro,
                ClienteId = paquete.ClienteId
            };
        }
    }
}