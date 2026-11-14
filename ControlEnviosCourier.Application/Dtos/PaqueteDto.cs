namespace ControlEnviosCourier.Application.Dtos
{
    public class PaqueteDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Peso { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int ClienteId { get; set; }
    }
}
