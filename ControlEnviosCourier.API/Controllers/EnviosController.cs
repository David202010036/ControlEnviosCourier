using ControlEnviosCourier.Application.Dtos;
using ControlEnviosCourier.Application.Models;
using ControlEnviosCourier.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControlEnviosCourier.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnviosController : ControllerBase
    {
        private readonly EnvioService service;

        public EnviosController(EnvioService service)
        {
            this.service = service;
        }

        [HttpGet]
        public ActionResult<ServiceResult<List<EnvioDto>>> ObtenerEnvios()
        {
            var resultado = service.ObtenerEnvios();
            return StatusCode(resultado.StatusCode, resultado);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ServiceResult<EnvioDto>> ObtenerEnvio(int id)
        {
            var resultado = service.ObtenerEnvio(id);
            return StatusCode(resultado.StatusCode, resultado);
        }

        [HttpPost]
        public ActionResult<ServiceResult<EnvioDto>> RegistrarEnvio(GuardarEnvioDto datos)
        {
            var resultado = service.RegistrarEnvio(datos);
            if (!resultado.Success)
                return StatusCode(resultado.StatusCode, resultado);
            return CreatedAtAction(nameof(ObtenerEnvio), new { id = resultado.Data!.Id }, resultado);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ServiceResult<EnvioDto>> ActualizarEnvio(int id, GuardarEnvioDto datos)
        {
            var resultado = service.ActualizarEnvio(id, datos);
            return StatusCode(resultado.StatusCode, resultado);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ServiceResult<bool>> EliminarEnvio(int id)
        {
            var resultado = service.EliminarEnvio(id);
            return StatusCode(resultado.StatusCode, resultado);
        }
    }
}
