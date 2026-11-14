using ControlEnviosCourier.Domain.Entities;
using ControlEnviosCourier.Infrastructure.Context;

namespace ControlEnviosCourier.Infrastructure.Repositories
{
    public class ClienteRepository : GenericRepository<Cliente>
    {
        public ClienteRepository(ApplicationDbContext context) : base(context) { }

        public bool ExisteCedula(string cedula, int? excluirId = null)
        {
            return context.Clientes.Any(c => c.Cedula == cedula && c.Id != excluirId);
        }

        public bool TienePaquetes(int clienteId)
        {
            return context.Paquetes.Any(p => p.ClienteId == clienteId);
        }
    }
}
