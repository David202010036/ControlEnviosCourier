using ControlEnviosCourier.Application.Dtos;
using ControlEnviosCourier.Application.Models;
using ControlEnviosCourier.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControlEnviosCourier.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaquetesController : ControllerBase
    {
        private readonly PaqueteService service;

        public PaquetesController(PaqueteService service)
        {
            this.service = service;
        }

        [HttpGet]
        public ActionResult<ServiceResult<List<PaqueteDto>>> ObtenerPaquetes()
        {
            var resultado = service.ObtenerPaquetes();
            return StatusCode(resultado.StatusCode, resultado);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ServiceResult<PaqueteDto>> ObtenerPaquete(int id)
        {
            var resultado = service.ObtenerPaquete(id);
            return StatusCode(resultado.StatusCode, resultado);
        }

        [HttpPost]
        public ActionResult<ServiceResult<PaqueteDto>> RegistrarPaquete(GuardarPaqueteDto datos)
        {
            var resultado = service.RegistrarPaquete(datos);
            if (!resultado.Success)
                return StatusCode(resultado.StatusCode, resultado);
            return CreatedAtAction(nameof(ObtenerPaquete), new { id = resultado.Data!.Id }, resultado);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ServiceResult<PaqueteDto>> ActualizarPaquete(int id, GuardarPaqueteDto datos)
        {
            var resultado = service.ActualizarPaquete(id, datos);
            return StatusCode(resultado.StatusCode, resultado);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ServiceResult<bool>> EliminarPaquete(int id)
        {
            var resultado = service.EliminarPaquete(id);
            return StatusCode(resultado.StatusCode, resultado);
        }
    }
}
