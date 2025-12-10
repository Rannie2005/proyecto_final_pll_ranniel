using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EstudioGrabacion.Data;
using EstudioGrabacion.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstudioGrabacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServiciosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Servicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetServicios()
        {
            var servicios = await _context.Servicios
                .Select(s => new
                {
                    s.Id,
                    s.Nombre,
                    s.Descripcion,
                    s.Precio,
                    s.DuracionHoras,
                    s.Activo,
                    s.Categoria
                })
                .ToListAsync();

            return Ok(servicios);
        }

        // GET: api/Servicios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetServicio(int id)
        {
            var servicio = await _context.Servicios
                .Where(s => s.Id == id)
                .Select(s => new
                {
                    s.Id,
                    s.Nombre,
                    s.Descripcion,
                    s.Precio,
                    s.DuracionHoras,
                    s.Activo,
                    s.Categoria
                })
                .FirstOrDefaultAsync();

            if (servicio == null)
            {
                return NotFound(new { message = "Servicio no encontrado" });
            }

            return servicio;
        }

        // POST: api/Servicios
        [HttpPost]
        public async Task<ActionResult<Servicio>> PostServicio(Servicio servicio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServicio", new { id = servicio.Id }, servicio);
        }

        // PUT: api/Servicios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicio(int id, Servicio servicio)
        {
            if (id != servicio.Id)
            {
                return BadRequest();
            }

            _context.Entry(servicio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicioExists(id))
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

        // DELETE: api/Servicios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicio(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
            {
                return NotFound();
            }

            // Verificar si el servicio está en uso en algún paquete
            var enPaquete = await _context.ServicioPaquetes
                .AnyAsync(sp => sp.ServicioId == id);

            if (enPaquete)
            {
                return BadRequest(new { message = "No se puede eliminar el servicio porque está incluido en uno o más paquetes" });
            }

            // Verificar si el servicio tiene sesiones
            var tieneSesiones = await _context.Sesiones
                .AnyAsync(s => s.ServicioId == id);

            if (tieneSesiones)
            {
                return BadRequest(new { message = "No se puede eliminar el servicio porque tiene sesiones registradas" });
            }

            _context.Servicios.Remove(servicio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServicioExists(int id)
        {
            return _context.Servicios.Any(e => e.Id == id);
        }
    }
}