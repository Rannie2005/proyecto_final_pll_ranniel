using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace EstudioGrabacion.Models
{
    public class Sesion
    {
        [Key]
        public int Id { get; set; }

        public DateTime FechaHoraInicio { get; set; } = DateTime.Now;
        public DateTime FechaHoraFin { get; set; } = DateTime.Now.AddHours(1);

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostoTotal { get; set; }

        [Required]
        public string Estado { get; set; } = "Pendiente";

        [StringLength(500)]
        public string? Notas { get; set; }

        // Nuevos campos para información del cliente
        [StringLength(100)]
        public string? NombreCliente { get; set; }

        [Phone]
        [StringLength(20)]
        public string? TelefonoCliente { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? EmailCliente { get; set; }

        [StringLength(200)]
        public string? DireccionCliente { get; set; }

        // Relación con Cliente (Usuario) - Opcional
        public string? UsuarioId { get; set; }
        public virtual IdentityUser? Usuario { get; set; }

        // Relación con Ingeniero
        [Required]
        public int IngenieroId { get; set; }
        public virtual Ingeniero? Ingeniero { get; set; }

        // Relación con Servicio
        [Required]
        public int ServicioId { get; set; }
        public virtual Servicio? Servicio { get; set; }
    }
}