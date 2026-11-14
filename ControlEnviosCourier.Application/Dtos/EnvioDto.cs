namespace ControlEnviosCourier.Application.Dtos
{
    public class EnvioDto
    {
        public int Id { get; set; }
        public string CodigoSeguimiento { get; set; } = string.Empty;
        public int PaqueteId { get; set; }
        public int EstadoEnvioId { get; set; }
        public decimal Tarifa { get; set; }
        public DateTime FechaEnvio { get; set; }
        public DateTime? FechaEntrega { get; set; }
    }
}
