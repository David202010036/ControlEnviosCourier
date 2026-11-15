using System.ComponentModel.DataAnnotations;

namespace ControlEnviosCourier.Application.Dtos
{
    public class GuardarClienteDto
    {
        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required, StringLength(25)]
        public string Cedula { get; set; } = string.Empty;

        [Required, StringLength(30)]
        public string Telefono { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(150)]
        public string Correo { get; set; } = string.Empty;

        [Required, StringLength(250)]
        public string Direccion { get; set; } = string.Empty;
    }
}
