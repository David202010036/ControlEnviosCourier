using ControlEnviosCourier.Application.Dtos;
using ControlEnviosCourier.Domain.Entities;
using ControlEnviosCourier.Infrastructure.Repositories;
using System.ComponentModel.DataAnnotations;
using ControlEnviosCourier.Application.Models;

namespace ControlEnviosCourier.Application.Services
{

    public class EstadoEnvioService
    {
        private readonly GenericRepository<EstadoEnvio> repository;
        private readonly GenericRepository<Envio> envios;

        public EstadoEnvioService(GenericRepository<EstadoEnvio> repository,
            GenericRepository<Envio> envios)
        {
            this.repository = repository;
            this.envios = envios;
        }

        public ServiceResult<List<EstadoEnvioDto>> ObtenerEstados()
        {
            return ServiceResult<List<EstadoEnvioDto>>.SuccessResponse(repository.GetAll().Select(ConvertirDto).ToList());
        }

        public ServiceResult<EstadoEnvioDto> ObtenerEstado(int id)
        {
            var estado = repository.GetById(id);
            if (estado == null) return ServiceResult<EstadoEnvioDto>.ErrorResponse("Estado no encontrado.", 404);
            return ServiceResult<EstadoEnvioDto>.SuccessResponse(ConvertirDto(estado));
        }

        public ServiceResult<EstadoEnvioDto> RegistrarEstado(GuardarEstadoEnvioDto datos)
        {
            var error = Validar(datos);
            if (error != null)
                return ServiceResult<EstadoEnvioDto>.ErrorResponse(error);

            if (repository.Any(e => e.Nombre == datos.Nombre))
                return ServiceResult<EstadoEnvioDto>.ErrorResponse("Ya existe un estado con ese nombre.", 409);
            var estado = repository.Create(new EstadoEnvio { Nombre = datos.Nombre });
            return ServiceResult<EstadoEnvioDto>.SuccessResponse(ConvertirDto(estado), 201, "Registro creado.");
        }

        public ServiceResult<EstadoEnvioDto> ActualizarEstado(int id, GuardarEstadoEnvioDto datos)
        {
            var error = Validar(datos);
            if (error != null)
                return ServiceResult<EstadoEnvioDto>.ErrorResponse(error);

            var estado = repository.GetById(id);
            if (estado == null) return ServiceResult<EstadoEnvioDto>.ErrorResponse("Estado no encontrado.", 404);
            if (repository.Any(e => e.Nombre == datos.Nombre && e.Id != id))
                return ServiceResult<EstadoEnvioDto>.ErrorResponse("Ya existe otro estado con ese nombre.", 409);
            estado.Nombre = datos.Nombre;
            repository.Update(estado);
            return ServiceResult<EstadoEnvioDto>.SuccessResponse(ConvertirDto(estado), message: "Registro actualizado.");
        }

        public ServiceResult<bool> EliminarEstado(int id)
        {
            var estado = repository.GetById(id);
            if (estado == null) return ServiceResult<bool>.ErrorResponse("Estado no encontrado.", 404);
            if (envios.Any(e => e.EstadoEnvioId == id))
                return ServiceResult<bool>.ErrorResponse("No se puede eliminar un estado utilizado por envíos.", 409);
            repository.Delete(estado);
            return ServiceResult<bool>.SuccessResponse(true, message: "Registro eliminado.");
        }

        private static EstadoEnvioDto ConvertirDto(EstadoEnvio estado)
        {
            return new EstadoEnvioDto { Id = estado.Id, Nombre = estado.Nombre };
        }
        private static string? Validar(GuardarEstadoEnvioDto datos)
        {
            if (datos == null) return "Debe indicar los datos del registro.";
            var errores = new List<ValidationResult>();
            if (Validator.TryValidateObject(datos, new ValidationContext(datos), errores, true))
                return null;
            return string.Join(" ", errores.Select(e => e.ErrorMessage));
        }
    }
}
