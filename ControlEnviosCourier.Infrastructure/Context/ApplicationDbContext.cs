using ControlEnviosCourier.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ControlEnviosCourier.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Paquete> Paquetes { get; set; }
        public DbSet<Envio> Envios { get; set; }
        public DbSet<EstadoEnvio> EstadosEnvio { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var cliente = modelBuilder.Entity<Cliente>();
            cliente.ToTable("Clientes");
            cliente.Property(c => c.Nombre).HasMaxLength(100);
            cliente.Property(c => c.Apellido).HasMaxLength(100);
            cliente.Property(c => c.Cedula).HasMaxLength(25);
            cliente.Property(c => c.Telefono).HasMaxLength(30);
            cliente.Property(c => c.Correo).HasMaxLength(150);
            cliente.Property(c => c.Direccion).HasMaxLength(250);
            cliente.HasIndex(c => c.Cedula).IsUnique();

            var paquete = modelBuilder.Entity<Paquete>();
            paquete.ToTable("Paquetes", t => t.HasCheckConstraint("CK_Paquetes_Peso", "[Peso] > 0"));
            paquete.Property(p => p.Descripcion).HasMaxLength(250);
            paquete.Property(p => p.Peso).HasPrecision(10, 2);
            paquete.Property(p => p.FechaRegistro).HasDefaultValueSql("SYSDATETIME()");
            paquete.HasOne(p => p.Cliente).WithMany()
                .HasForeignKey(p => p.ClienteId).OnDelete(DeleteBehavior.NoAction);

            var estado = modelBuilder.Entity<EstadoEnvio>();
            estado.ToTable("EstadosEnvio");
            estado.Property(e => e.Nombre).HasMaxLength(50);
            estado.HasIndex(e => e.Nombre).IsUnique();

            var envio = modelBuilder.Entity<Envio>();
            envio.ToTable("Envios", t =>
            {
                t.HasCheckConstraint("CK_Envios_Tarifa", "[Tarifa] >= 0");
                t.HasCheckConstraint("CK_Envios_Fechas", "[FechaEntrega] IS NULL OR [FechaEntrega] >= [FechaEnvio]");
            });
            envio.Property(e => e.CodigoSeguimiento).HasMaxLength(50);
            envio.HasIndex(e => e.CodigoSeguimiento).IsUnique();
            envio.Property(e => e.Tarifa).HasPrecision(10, 2);
            envio.HasOne(e => e.Paquete).WithMany()
                .HasForeignKey(e => e.PaqueteId).OnDelete(DeleteBehavior.NoAction);
            envio.HasOne(e => e.EstadoEnvio).WithMany()
                .HasForeignKey(e => e.EstadoEnvioId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
