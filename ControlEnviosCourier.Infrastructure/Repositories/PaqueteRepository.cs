using ControlEnviosCourier.Domain.Entities;
using ControlEnviosCourier.Infrastructure.Context;

namespace ControlEnviosCourier.Infrastructure.Repositories
{
    public class PaqueteRepository : GenericRepository<Paquete>
    {
        public PaqueteRepository(ApplicationDbContext context) : base(context) { }

        public bool ExisteCliente(int clienteId)
        {
            return context.Clientes.Any(c => c.Id == clienteId);
        }

        public bool TieneEnvios(int paqueteId)
        {
            return context.Envios.Any(e => e.PaqueteId == paqueteId);
        }
    }
}
