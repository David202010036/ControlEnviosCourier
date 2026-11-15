using ControlEnviosCourier.Application.Dtos;
using ControlEnviosCourier.Domain.Entities;
using ControlEnviosCourier.Infrastructure.Repositories;
using System.ComponentModel.DataAnnotations;
using ControlEnviosCourier.Application.Models;

namespace ControlEnviosCourier.Application.Services
{

    public class EnvioService
    {
        private readonly GenericRepository<Envio> repository;
        private readonly PaqueteRepository paquetes;
        private readonly GenericRepository<EstadoEnvio> estados;

        public EnvioService(GenericRepository<Envio> repository,
            PaqueteRepository paquetes, GenericRepository<EstadoEnvio> estados)
        {
            this.repository = repository;
            this.paquetes = paquetes;
            this.estados = estados;
        }

        public ServiceResult<List<EnvioDto>> ObtenerEnvios()
        {
            return ServiceResult<List<EnvioDto>>.SuccessResponse(repository.GetAll().Select(ConvertirDto).ToList());
        }

        public ServiceResult<EnvioDto> ObtenerEnvio(int id)
        {
            var envio = repository.GetById(id);
            if (envio == null) return ServiceResult<EnvioDto>.ErrorResponse("Envío no encontrado.", 404);
            return ServiceResult<EnvioDto>.SuccessResponse(ConvertirDto(envio));
        }

        public ServiceResult<EnvioDto> RegistrarEnvio(GuardarEnvioDto datos)
        {
            var error = Validar(datos);
            if (error != null)
                return ServiceResult<EnvioDto>.ErrorResponse(error);

            if (paquetes.GetById(datos.PaqueteId) == null)
                return ServiceResult<EnvioDto>.ErrorResponse("El paquete indicado no existe.");
            if (estados.GetById(datos.EstadoEnvioId) == null)
                return ServiceResult<EnvioDto>.ErrorResponse("El estado indicado no existe.");
            var envio = repository.Create(new Envio
            {
                CodigoSeguimiento = Guid.NewGuid().ToString("N"),
                PaqueteId = datos.PaqueteId,
                EstadoEnvioId = datos.EstadoEnvioId,
                Tarifa = datos.Tarifa,
                FechaEnvio = datos.FechaEnvio,
                FechaEntrega = datos.FechaEntrega
            });
            return ServiceResult<EnvioDto>.SuccessResponse(ConvertirDto(envio), 201, "Registro creado.");
        }

        public ServiceResult<EnvioDto> ActualizarEnvio(int id, GuardarEnvioDto datos)
        {
            var error = Validar(datos);
            if (error != null)
                return ServiceResult<EnvioDto>.ErrorResponse(error);

            var envio = repository.GetById(id);
            if (envio == null) return ServiceResult<EnvioDto>.ErrorResponse("Envío no encontrado.", 404);
            if (paquetes.GetById(datos.PaqueteId) == null)
                return ServiceResult<EnvioDto>.ErrorResponse("El paquete indicado no existe.");
            if (estados.GetById(datos.EstadoEnvioId) == null)
                return ServiceResult<EnvioDto>.ErrorResponse("El estado indicado no existe.");
            envio.PaqueteId = datos.PaqueteId;
            envio.EstadoEnvioId = datos.EstadoEnvioId;
            envio.Tarifa = datos.Tarifa;
            envio.FechaEnvio = datos.FechaEnvio;
            envio.FechaEntrega = datos.FechaEntrega;
            repository.Update(envio);
            return ServiceResult<EnvioDto>.SuccessResponse(ConvertirDto(envio), message: "Registro actualizado.");
        }

        public ServiceResult<bool> EliminarEnvio(int id)
        {
            var envio = repository.GetById(id);
            if (envio == null) return ServiceResult<bool>.ErrorResponse("Envío no encontrado.", 404);
            repository.Delete(envio);
            return ServiceResult<bool>.SuccessResponse(true, message: "Registro eliminado.");
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
        private static string? Validar(GuardarEnvioDto datos)
        {
            if (datos == null) return "Debe indicar los datos del registro.";
            var errores = new List<ValidationResult>();
            if (Validator.TryValidateObject(datos, new ValidationContext(datos), errores, true))
                return null;
            return string.Join(" ", errores.Select(e => e.ErrorMessage));
        }
    }
}
