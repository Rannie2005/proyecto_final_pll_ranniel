using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstudioGrabacion.Models
{
    public class Ingeniero
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(100)]
        public string Especialidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TarifaPorHora { get; set; }

        public bool Disponible { get; set; } = true;
        public bool Activo { get; set; } = true; // Añade esta propiedad

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string? Telefono { get; set; }

        // Relación con Sesiones
        public virtual ICollection<Sesion>? Sesiones { get; set; }
    }
}