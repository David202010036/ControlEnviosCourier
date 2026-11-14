using ControlEnviosCourier.Domain.Core;

namespace ControlEnviosCourier.Domain.Entities
{
    public class Paquete : BaseEntity
    {
        public string Descripcion { get; set; } = string.Empty;
        public decimal Peso { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; } = null!;
    }
}
