using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EstudioGrabacion.Data;
using EstudioGrabacion.Models;
using System.Linq;
using System.Threading.Tasks;

namespace EstudioGrabacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaquetesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaquetesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Paquetes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetPaquetes()
        {
            var paquetes = await _context.Paquetes
                .Include(p => p.ServicioPaquetes)
                    .ThenInclude(sp => sp.Servicio)
                .Select(p => new
                {
                    p.Id,
                    p.Nombre,
                    p.Descripcion,
                    p.PrecioTotal,
                    p.DuracionTotalHoras,
                    p.Activo,
                    Servicios = p.ServicioPaquetes.Select(sp => new
                    {
                        sp.Servicio.Id,
                        sp.Servicio.Nombre,
                        sp.Servicio.Precio,
                        sp.Servicio.DuracionHoras
                    }).ToList()
                })
                .ToListAsync();

            return Ok(paquetes);
        }

        // GET: api/Paquetes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetPaquete(int id)
        {
            var paquete = await _context.Paquetes
                .Include(p => p.ServicioPaquetes)
                    .ThenInclude(sp => sp.Servicio)
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.Nombre,
                    p.Descripcion,
                    p.PrecioTotal,
                    p.DuracionTotalHoras,
                    p.Activo,
                    Servicios = p.ServicioPaquetes.Select(sp => new
                    {
                        sp.Servicio.Id,
                        sp.Servicio.Nombre,
                        sp.Servicio.Precio,
                        sp.Servicio.DuracionHoras
                    }).ToList(),
                    ServicioIds = p.ServicioPaquetes.Select(sp => sp.ServicioId).ToList()
                })
                .FirstOrDefaultAsync();

            if (paquete == null)
            {
                return NotFound(new { message = "Paquete no encontrado" });
            }

            return paquete;
        }

        // POST: api/Paquetes
        [HttpPost]
        public async Task<ActionResult<Paquete>> PostPaquete(Paquete paquete)
        {
            // Validar servicios seleccionados
            if (paquete.ServicioIds == null || !paquete.ServicioIds.Any())
            {
                return BadRequest(new { message = "Debe seleccionar al menos un servicio" });
            }

            // Verificar que todos los servicios existan
            var serviciosExistentes = await _context.Servicios
                .Where(s => paquete.ServicioIds.Contains(s.Id))
                .ToListAsync();

            if (serviciosExistentes.Count != paquete.ServicioIds.Count)
            {
                return BadRequest(new { message = "Uno o más servicios no existen" });
            }

            // Crear el paquete
            var nuevoPaquete = new Paquete
            {
                Nombre = paquete.Nombre,
                Descripcion = paquete.Descripcion,
                PrecioTotal = paquete.PrecioTotal,
                DuracionTotalHoras = paquete.DuracionTotalHoras,
                Activo = paquete.Activo
            };

            _context.Paquetes.Add(nuevoPaquete);
            await _context.SaveChangesAsync();

            // Agregar relaciones con servicios
            foreach (var servicioId in paquete.ServicioIds)
            {
                var servicioPaquete = new ServicioPaquete
                {
                    PaqueteId = nuevoPaquete.Id,
                    ServicioId = servicioId
                };
                _context.ServicioPaquetes.Add(servicioPaquete);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaquete", new { id = nuevoPaquete.Id }, nuevoPaquete);
        }

        // PUT: api/Paquetes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaquete(int id, Paquete paquete)
        {
            if (id != paquete.Id)
            {
                return BadRequest();
            }

            var paqueteExistente = await _context.Paquetes
                .Include(p => p.ServicioPaquetes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (paqueteExistente == null)
            {
                return NotFound();
            }

            // Actualizar propiedades básicas
            paqueteExistente.Nombre = paquete.Nombre;
            paqueteExistente.Descripcion = paquete.Descripcion;
            paqueteExistente.PrecioTotal = paquete.PrecioTotal;
            paqueteExistente.DuracionTotalHoras = paquete.DuracionTotalHoras;
            paqueteExistente.Activo = paquete.Activo;

            // Actualizar servicios si se proporcionan
            if (paquete.ServicioIds != null && paquete.ServicioIds.Any())
            {
                // Eliminar relaciones existentes
                _context.ServicioPaquetes.RemoveRange(paqueteExistente.ServicioPaquetes);

                // Agregar nuevas relaciones
                foreach (var servicioId in paquete.ServicioIds)
                {
                    var servicioPaquete = new ServicioPaquete
                    {
                        PaqueteId = id,
                        ServicioId = servicioId
                    };
                    _context.ServicioPaquetes.Add(servicioPaquete);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaqueteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Paquetes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaquete(int id)
        {
            var paquete = await _context.Paquetes
                .Include(p => p.ServicioPaquetes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (paquete == null)
            {
                return NotFound();
            }

            // Eliminar primero las relaciones
            _context.ServicioPaquetes.RemoveRange(paquete.ServicioPaquetes);

            // Eliminar el paquete
            _context.Paquetes.Remove(paquete);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaqueteExists(int id)
        {
            return _context.Paquetes.Any(e => e.Id == id);
        }
    }
}