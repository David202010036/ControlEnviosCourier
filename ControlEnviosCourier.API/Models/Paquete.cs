namespace ControlEnviosCourier.API.Models
{
    public class Paquete
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Peso { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; } = null!;
    }
}