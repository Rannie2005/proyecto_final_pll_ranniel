using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstudioGrabacion.Models
{
    public class Servicio
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del servicio es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es requerido")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 10000, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La duración es requerida")]
        [Range(1, 24, ErrorMessage = "La duración debe ser entre 1 y 24 horas")]
        public int DuracionHoras { get; set; } = 1; // Agregado: duración del servicio

        [Required]
        public bool Activo { get; set; } = true;

        [StringLength(50)]
        public string Categoria { get; set; } = "General";

        // Relación con Sesiones
        public virtual ICollection<Sesion> Sesiones { get; set; } = new List<Sesion>();

        // Relación con Paquetes (muchos a muchos)
        public virtual ICollection<ServicioPaquete> ServicioPaquetes { get; set; } = new List<ServicioPaquete>();

        public Servicio() { }

        public Servicio(string nombre, decimal precio, int duracionHoras = 1)
        {
            Nombre = nombre;
            Precio = precio;
            DuracionHoras = duracionHoras;
        }
    }
}