using ControlEnviosCourier.Infrastructure.Repositories;
using ControlEnviosCourier.Domain.Entities;
using ControlEnviosCourier.Application.Dtos;
using System.ComponentModel.DataAnnotations;
using ControlEnviosCourier.Application.Models;

namespace ControlEnviosCourier.Application.Services
{

    public class PaqueteService
    {
        private readonly PaqueteRepository repository;

        public PaqueteService(PaqueteRepository repository)
        {
            this.repository = repository;
        }

        public ServiceResult<List<PaqueteDto>> ObtenerPaquetes()
        {
            var paquetes = repository.GetAll();
            return ServiceResult<List<PaqueteDto>>.SuccessResponse(paquetes.Select(ConvertirDto).ToList());
        }

        public ServiceResult<PaqueteDto> ObtenerPaquete(int id)
        {
            var paquete = repository.GetById(id);

            if (paquete == null)
                return ServiceResult<PaqueteDto>.ErrorResponse("Paquete no encontrado.", 404);

            return ServiceResult<PaqueteDto>.SuccessResponse(ConvertirDto(paquete));
        }

        public ServiceResult<PaqueteDto> RegistrarPaquete(
            GuardarPaqueteDto datos)
        {
            var error = Validar(datos);
            if (error != null)
                return ServiceResult<PaqueteDto>.ErrorResponse(error);

            if (!repository.ExisteCliente(datos.ClienteId))
                return ServiceResult<PaqueteDto>.ErrorResponse("El cliente indicado no existe.");

            var paquete = new Paquete
            {
                Descripcion = datos.Descripcion,
                Peso = datos.Peso,
                ClienteId = datos.ClienteId,
                FechaRegistro = DateTime.Now
            };

            repository.Create(paquete);

            return ServiceResult<PaqueteDto>.SuccessResponse(ConvertirDto(paquete), 201, "Registro creado.");
        }

        public ServiceResult<PaqueteDto> ActualizarPaquete(
            int id, GuardarPaqueteDto datos)
        {
            var error = Validar(datos);
            if (error != null)
                return ServiceResult<PaqueteDto>.ErrorResponse(error);

            var paquete = repository.GetById(id);

            if (paquete == null)
                return ServiceResult<PaqueteDto>.ErrorResponse("Paquete no encontrado.", 404);

            if (!repository.ExisteCliente(datos.ClienteId))
                return ServiceResult<PaqueteDto>.ErrorResponse("El cliente indicado no existe.");

            paquete.Descripcion = datos.Descripcion;
            paquete.Peso = datos.Peso;
            paquete.ClienteId = datos.ClienteId;

            repository.Update(paquete);

            return ServiceResult<PaqueteDto>.SuccessResponse(ConvertirDto(paquete), message: "Registro actualizado.");
        }

        public ServiceResult<bool> EliminarPaquete(int id)
        {
            var paquete = repository.GetById(id);

            if (paquete == null)
                return ServiceResult<bool>.ErrorResponse("Paquete no encontrado.", 404);

            if (repository.TieneEnvios(id))
                return ServiceResult<bool>.ErrorResponse("No se puede eliminar un paquete con envíos asociados.", 409);

            repository.Delete(paquete);

            return ServiceResult<bool>.SuccessResponse(true, message: "Registro eliminado.");
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
        private static string? Validar(GuardarPaqueteDto datos)
        {
            if (datos == null) return "Debe indicar los datos del registro.";
            var errores = new List<ValidationResult>();
            if (Validator.TryValidateObject(datos, new ValidationContext(datos), errores, true))
                return null;
            return string.Join(" ", errores.Select(e => e.ErrorMessage));
        }
    }
}
