using System.ComponentModel.DataAnnotations;

namespace ControlEnviosCourier.API.Models.Dtos
{
    public class GuardarPaqueteDto
    {
        [Required, StringLength(250)]
        public string Descripcion { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "99999999.99")]
        public decimal Peso { get; set; }

        [Range(1, int.MaxValue)]
        public int ClienteId { get; set; }
    }
}