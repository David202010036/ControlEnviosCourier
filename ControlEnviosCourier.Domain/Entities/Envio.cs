using ControlEnviosCourier.Domain.Core;

namespace ControlEnviosCourier.Domain.Entities
{
    public class Envio : BaseEntity
    {
        public string CodigoSeguimiento { get; set; } = string.Empty;
        public int PaqueteId { get; set; }
        public int EstadoEnvioId { get; set; }
        public decimal Tarifa { get; set; }
        public DateTime FechaEnvio { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public Paquete Paquete { get; set; } = null!;
        public EstadoEnvio EstadoEnvio { get; set; } = null!;
    }
}
