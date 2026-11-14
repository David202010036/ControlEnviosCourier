using ControlEnviosCourier.Infrastructure.Repositories;
using ControlEnviosCourier.Domain.Entities;
using ControlEnviosCourier.Application.Dtos;
using System.ComponentModel.DataAnnotations;
using ControlEnviosCourier.Application.Models;

namespace ControlEnviosCourier.Application.Services
{

    public class ClienteService
    {
        private readonly ClienteRepository repository;

        public ClienteService(ClienteRepository repository)
        {
            this.repository = repository;
        }

        public ServiceResult<List<ClienteDto>> ObtenerClientes()
        {
            var clientes = repository.GetAll();
            return ServiceResult<List<ClienteDto>>.SuccessResponse(clientes.Select(ConvertirDto).ToList());
        }

        public ServiceResult<ClienteDto> ObtenerCliente(int id)
        {
            var cliente = repository.GetById(id);

            if (cliente == null)
                return ServiceResult<ClienteDto>.ErrorResponse("Cliente no encontrado.", 404);

            return ServiceResult<ClienteDto>.SuccessResponse(ConvertirDto(cliente));
        }

        public ServiceResult<ClienteDto> RegistrarCliente(
            GuardarClienteDto datos)
        {
            var error = Validar(datos);
            if (error != null)
                return ServiceResult<ClienteDto>.ErrorResponse(error);

            if (repository.ExisteCedula(datos.Cedula))
                return ServiceResult<ClienteDto>.ErrorResponse("Ya existe un cliente con esa cédula.", 409);

            var cliente = new Cliente
            {
                Nombre = datos.Nombre,
                Apellido = datos.Apellido,
                Cedula = datos.Cedula,
                Telefono = datos.Telefono,
                Correo = datos.Correo,
                Direccion = datos.Direccion
            };

            repository.Create(cliente);

            return ServiceResult<ClienteDto>.SuccessResponse(ConvertirDto(cliente), 201, "Registro creado.");
        }

        public ServiceResult<ClienteDto> ActualizarCliente(
            int id, GuardarClienteDto datos)
        {
            var error = Validar(datos);
            if (error != null)
                return ServiceResult<ClienteDto>.ErrorResponse(error);

            var cliente = repository.GetById(id);

            if (cliente == null)
                return ServiceResult<ClienteDto>.ErrorResponse("Cliente no encontrado.", 404);

            if (repository.ExisteCedula(datos.Cedula, id))
            {
                return ServiceResult<ClienteDto>.ErrorResponse("Ya existe otro cliente con esa cédula.", 409);
            }

            cliente.Nombre = datos.Nombre;
            cliente.Apellido = datos.Apellido;
            cliente.Cedula = datos.Cedula;
            cliente.Telefono = datos.Telefono;
            cliente.Correo = datos.Correo;
            cliente.Direccion = datos.Direccion;

            repository.Update(cliente);

            return ServiceResult<ClienteDto>.SuccessResponse(ConvertirDto(cliente), message: "Registro actualizado.");
        }

        public ServiceResult<bool> EliminarCliente(int id)
        {
            var cliente = repository.GetById(id);

            if (cliente == null)
                return ServiceResult<bool>.ErrorResponse("Cliente no encontrado.", 404);

            if (repository.TienePaquetes(id))
            {
                return ServiceResult<bool>.ErrorResponse("No se puede eliminar un cliente con paquetes asociados.", 409);
            }

            repository.Delete(cliente);

            return ServiceResult<bool>.SuccessResponse(true, message: "Registro eliminado.");
        }

        private static ClienteDto ConvertirDto(Cliente cliente)
        {
            return new ClienteDto
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Cedula = cliente.Cedula,
                Telefono = cliente.Telefono,
                Correo = cliente.Correo,
                Direccion = cliente.Direccion
            };
        }
        private static string? Validar(GuardarClienteDto datos)
        {
            if (datos == null) return "Debe indicar los datos del registro.";
            var errores = new List<ValidationResult>();
            if (Validator.TryValidateObject(datos, new ValidationContext(datos), errores, true))
                return null;
            return string.Join(" ", errores.Select(e => e.ErrorMessage));
        }
    }
}
