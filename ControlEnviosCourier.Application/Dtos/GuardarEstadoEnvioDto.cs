using System.ComponentModel.DataAnnotations;

namespace ControlEnviosCourier.Application.Dtos
{
    public class GuardarEstadoEnvioDto
    {
        [Required, StringLength(50)]
        public string Nombre { get; set; } = string.Empty;
    }
}
