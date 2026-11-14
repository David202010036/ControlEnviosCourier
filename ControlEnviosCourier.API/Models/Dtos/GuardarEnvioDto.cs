using System.ComponentModel.DataAnnotations;

namespace ControlEnviosCourier.API.Models.Dtos
{
    public class GuardarEnvioDto : IValidatableObject
    {
        [Range(1, int.MaxValue)]
        public int PaqueteId { get; set; }

        [Range(1, int.MaxValue)]
        public int EstadoEnvioId { get; set; }

        [Range(typeof(decimal), "0", "99999999.99")]
        public decimal Tarifa { get; set; }

        public DateTime FechaEnvio { get; set; } = DateTime.Now;
        public DateTime? FechaEntrega { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaEnvio == default)
                yield return new ValidationResult("La fecha de envío es obligatoria.", new[] { nameof(FechaEnvio) });
            if (FechaEntrega.HasValue && FechaEntrega < FechaEnvio)
                yield return new ValidationResult("La entrega no puede ser anterior al envío.", new[] { nameof(FechaEntrega) });
        }
    }
}
