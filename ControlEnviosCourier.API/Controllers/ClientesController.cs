using ControlEnviosCourier.Application.Dtos;
using ControlEnviosCourier.Application.Models;
using ControlEnviosCourier.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControlEnviosCourier.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService service;

        public ClientesController(ClienteService service)
        {
            this.service = service;
        }

        [HttpGet]
        public ActionResult<ServiceResult<List<ClienteDto>>> ObtenerClientes()
        {
            var resultado = service.ObtenerClientes();
            return StatusCode(resultado.StatusCode, resultado);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ServiceResult<ClienteDto>> ObtenerCliente(int id)
        {
            var resultado = service.ObtenerCliente(id);
            return StatusCode(resultado.StatusCode, resultado);
        }

        [HttpPost]
        public ActionResult<ServiceResult<ClienteDto>> RegistrarCliente(GuardarClienteDto datos)
        {
            var resultado = service.RegistrarCliente(datos);
            if (!resultado.Success)
                return StatusCode(resultado.StatusCode, resultado);
            return CreatedAtAction(nameof(ObtenerCliente), new { id = resultado.Data!.Id }, resultado);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ServiceResult<ClienteDto>> ActualizarCliente(int id, GuardarClienteDto datos)
        {
            var resultado = service.ActualizarCliente(id, datos);
            return StatusCode(resultado.StatusCode, resultado);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ServiceResult<bool>> EliminarCliente(int id)
        {
            var resultado = service.EliminarCliente(id);
            return StatusCode(resultado.StatusCode, resultado);
        }
    }
}
