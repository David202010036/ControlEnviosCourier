using ControlEnviosCourier.Domain.Core;

namespace ControlEnviosCourier.Domain.Entities
{
    public class EstadoEnvio : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
    }
}
