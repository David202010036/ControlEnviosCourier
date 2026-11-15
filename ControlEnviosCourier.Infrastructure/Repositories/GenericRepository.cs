using System.Linq.Expressions;
using ControlEnviosCourier.Domain.Core;
using ControlEnviosCourier.Infrastructure.Context;

namespace ControlEnviosCourier.Infrastructure.Repositories
{
    public class GenericRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext context;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<T> GetAll() => context.Set<T>().ToList();

        public T? GetById(int id) => context.Set<T>().Find(id);

        public bool Any(Expression<Func<T, bool>> filtro) => context.Set<T>().Any(filtro);

        public T Create(T entidad)
        {
            context.Set<T>().Add(entidad);
            context.SaveChanges();
            return entidad;
        }

        public void Update(T entidad)
        {
            context.Set<T>().Update(entidad);
            context.SaveChanges();
        }

        public void Delete(T entidad)
        {
            context.Set<T>().Remove(entidad);
            context.SaveChanges();
        }
    }
}
