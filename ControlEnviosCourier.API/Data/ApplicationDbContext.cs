using ControlEnviosCourier.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlEnviosCourier.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Paquete> Paquetes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<Paquete>().ToTable("Paquetes");

            modelBuilder.Entity<Paquete>()
                .Property(p => p.Peso)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Paquete>()
                .HasOne(p => p.Cliente)
                .WithMany()
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}