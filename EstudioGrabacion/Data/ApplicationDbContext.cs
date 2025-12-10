using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EstudioGrabacion.Models;

namespace EstudioGrabacion.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Sesion> Sesiones { get; set; }
        public DbSet<Ingeniero> Ingenieros { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Paquete> Paquetes { get; set; }
        public DbSet<ServicioPaquete> ServicioPaquetes { get; set; } // NUEVO

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuración de Sesiones
            builder.Entity<Sesion>()
                .HasOne(s => s.Ingeniero)
                .WithMany(i => i.Sesiones)
                .HasForeignKey(s => s.IngenieroId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Sesion>()
                .HasOne(s => s.Servicio)
                .WithMany(serv => serv.Sesiones)
                .HasForeignKey(s => s.ServicioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Sesion>()
                .HasOne(s => s.Usuario)
                .WithMany()
                .HasForeignKey(s => s.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configuración de la relación muchos a muchos Paquete-Servicio
            builder.Entity<ServicioPaquete>()
                .HasKey(sp => new { sp.PaqueteId, sp.ServicioId });

            builder.Entity<ServicioPaquete>()
                .HasOne(sp => sp.Paquete)
                .WithMany(p => p.ServicioPaquetes)
                .HasForeignKey(sp => sp.PaqueteId);

            builder.Entity<ServicioPaquete>()
                .HasOne(sp => sp.Servicio)
                .WithMany(s => s.ServicioPaquetes)
                .HasForeignKey(sp => sp.ServicioId);

            // Configurar valor por defecto para Estado en Sesiones
            builder.Entity<Sesion>()
                .Property(s => s.Estado)
                .HasDefaultValue("Pendiente");

            // Índice para búsquedas rápidas
            builder.Entity<Ingeniero>()
                .HasIndex(i => i.Disponible);

            builder.Entity<Servicio>()
                .HasIndex(s => s.Activo);
        }
    }
}