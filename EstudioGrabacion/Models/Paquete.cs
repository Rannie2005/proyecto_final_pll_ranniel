using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstudioGrabacion.Models
{
    public class Paquete
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del paquete es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio total es requerido")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 10000, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal PrecioTotal { get; set; }

        [Required(ErrorMessage = "La duración total es requerida")]
        [Range(1, 100, ErrorMessage = "La duración debe ser entre 1 y 100 horas")]
        public int DuracionTotalHoras { get; set; }

        [Required]
        public bool Activo { get; set; } = true;

        // Relación muchos a muchos con Servicios
        public virtual ICollection<ServicioPaquete> ServicioPaquetes { get; set; } = new List<ServicioPaquete>();

        // Propiedad de navegación para servicios (no mapeada)
        [NotMapped]
        public List<int> ServicioIds { get; set; } = new List<int>();
    }

    // Clase intermedia para la relación muchos a muchos
    public class ServicioPaquete
    {
        public int PaqueteId { get; set; }
        public Paquete Paquete { get; set; }

        public int ServicioId { get; set; }
        public Servicio Servicio { get; set; }
    }
}