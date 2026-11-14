using ControlEnviosCourier.Application.Dtos;
using ControlEnviosCourier.Application.Models;
using ControlEnviosCourier.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControlEnviosCourier.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadosEnvioController : ControllerBase
    {
        private readonly EstadoEnvioService service;

        public EstadosEnvioController(EstadoEnvioService service)
        {
            this.service = service;
        }

        [HttpGet]
        public ActionResult<ServiceResult<List<EstadoEnvioDto>>> ObtenerEstados()
        {
            var resultado = service.ObtenerEstados();
            return StatusCode(resultado.StatusCode, resultado);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ServiceResult<EstadoEnvioDto>> ObtenerEstado(int id)
        {
            var resultado = service.ObtenerEstado(id);
            return StatusCode(resultado.StatusCode, resultado);
        }

        [HttpPost]
        public ActionResult<ServiceResult<EstadoEnvioDto>> RegistrarEstado(GuardarEstadoEnvioDto datos)
        {
            var resultado = service.RegistrarEstado(datos);
            if (!resultado.Success)
                return StatusCode(resultado.StatusCode, resultado);
            return CreatedAtAction(nameof(ObtenerEstado), new { id = resultado.Data!.Id }, resultado);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ServiceResult<EstadoEnvioDto>> ActualizarEstado(int id, GuardarEstadoEnvioDto datos)
        {
            var resultado = service.ActualizarEstado(id, datos);
            return StatusCode(resultado.StatusCode, resultado);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ServiceResult<bool>> EliminarEstado(int id)
        {
            var resultado = service.EliminarEstado(id);
            return StatusCode(resultado.StatusCode, resultado);
        }
    }
}
