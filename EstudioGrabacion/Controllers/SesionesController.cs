using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EstudioGrabacion.Data;
using EstudioGrabacion.Models;
using Microsoft.AspNetCore.Authorization;

namespace EstudioGrabacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SesionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SesionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class ReservaSesionDTO
        {
            public DateTime FechaHoraInicio { get; set; }
            public DateTime FechaHoraFin { get; set; }
            public decimal CostoTotal { get; set; }
            public string Estado { get; set; } = "Pendiente";
            public string? Notas { get; set; }
            public string? NombreCliente { get; set; }
            public string? TelefonoCliente { get; set; }
            public string? EmailCliente { get; set; }
            public string? DireccionCliente { get; set; }
            public int IngenieroId { get; set; }
            public int ServicioId { get; set; }
        }

        // GET: api/Sesiones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sesion>>> GetSesiones()
        {
            return await _context.Sesiones
                .Include(s => s.Ingeniero)
                .Include(s => s.Servicio)
                .Include(s => s.Usuario)
                .OrderByDescending(s => s.FechaHoraInicio)
                .ToListAsync();
        }

        // GET: api/Servicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetServicios()
        {
            try
            {
                var servicios = await _context.Servicios
                    .Where(s => s.Activo)
                    .Select(s => new
                    {
                        s.Id,
                        s.Nombre,
                        s.Descripcion,
                        s.Precio,
                        DuracionHoras = s.DuracionHoras, // Asegurar que esta propiedad existe
                        Activo = s.Activo, // Cambiar a mayúscula para consistencia
                        s.Categoria
                    })
                    .ToListAsync();

                return Ok(servicios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al cargar servicios", error = ex.Message });
            }
        }

        // POST: api/Sesiones
        [HttpPost]
        public async Task<ActionResult<Sesion>> PostSesion([FromBody] ReservaSesionDTO reservaDTO)
        {
            try
            {
                Console.WriteLine($"=== POST SESION RECIBIDA ===");
                Console.WriteLine($"IngenieroId: {reservaDTO.IngenieroId}");
                Console.WriteLine($"ServicioId: {reservaDTO.ServicioId}");
                Console.WriteLine($"FechaInicio: {reservaDTO.FechaHoraInicio}");
                Console.WriteLine($"FechaFin: {reservaDTO.FechaHoraFin}");

                // Validar que el ingeniero exista
                var ingeniero = await _context.Ingenieros.FindAsync(reservaDTO.IngenieroId);
                if (ingeniero == null)
                {
                    Console.WriteLine("ERROR: Ingeniero no encontrado");
                    return BadRequest(new { message = "El ingeniero seleccionado no existe." });
                }

                // Validar que el servicio exista
                var servicio = await _context.Servicios.FindAsync(reservaDTO.ServicioId);
                if (servicio == null)
                {
                    Console.WriteLine("ERROR: Servicio no encontrado");
                    return BadRequest(new { message = "El servicio seleccionado no existe." });
                }

                // Validar que no haya conflictos de horario con el ingeniero
                var conflicto = await _context.Sesiones.AnyAsync(s =>
                    s.IngenieroId == reservaDTO.IngenieroId &&
                    s.Estado != "Cancelada" &&
                    ((s.FechaHoraInicio <= reservaDTO.FechaHoraInicio && s.FechaHoraFin > reservaDTO.FechaHoraInicio) ||
                     (s.FechaHoraInicio < reservaDTO.FechaHoraFin && s.FechaHoraFin >= reservaDTO.FechaHoraFin) ||
                     (s.FechaHoraInicio >= reservaDTO.FechaHoraInicio && s.FechaHoraFin <= reservaDTO.FechaHoraFin)));

                if (conflicto)
                {
                    Console.WriteLine("ERROR: Conflicto de horario");
                    return BadRequest(new { message = "El ingeniero ya tiene una sesión programada en ese horario." });
                }

                // Crear la sesión a partir del DTO
                var sesion = new Sesion
                {
                    FechaHoraInicio = reservaDTO.FechaHoraInicio,
                    FechaHoraFin = reservaDTO.FechaHoraFin,
                    CostoTotal = reservaDTO.CostoTotal,
                    Estado = reservaDTO.Estado,
                    Notas = reservaDTO.Notas,
                    NombreCliente = reservaDTO.NombreCliente,
                    TelefonoCliente = reservaDTO.TelefonoCliente,
                    EmailCliente = reservaDTO.EmailCliente,
                    DireccionCliente = reservaDTO.DireccionCliente,
                    IngenieroId = reservaDTO.IngenieroId,
                    ServicioId = reservaDTO.ServicioId
                };

                // Asignar usuario actual si está autenticado
                if (User.Identity.IsAuthenticated)
                {
                    sesion.UsuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                }

                _context.Sesiones.Add(sesion);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Sesión creada con ID: {sesion.Id}");

                // Cargar relaciones para la respuesta
                var sesionCreada = await _context.Sesiones
                    .Include(s => s.Ingeniero)
                    .Include(s => s.Servicio)
                    .FirstOrDefaultAsync(s => s.Id == sesion.Id);

                return Ok(new
                {
                    message = "Sesión creada exitosamente",
                    sesion = new
                    {
                        id = sesionCreada.Id,
                        fechaHoraInicio = sesionCreada.FechaHoraInicio,
                        fechaHoraFin = sesionCreada.FechaHoraFin,
                        costoTotal = sesionCreada.CostoTotal,
                        estado = sesionCreada.Estado,
                        nombreCliente = sesionCreada.NombreCliente,
                        ingeniero = sesionCreada.Ingeniero?.Nombre,
                        servicio = sesionCreada.Servicio?.Nombre
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCIÓN: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                return StatusCode(500, new
                {
                    message = "Error interno del servidor",
                    error = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        // PUT: api/Sesiones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSesion(int id, Sesion sesion)
        {
            if (id != sesion.Id)
            {
                return BadRequest();
            }

            _context.Entry(sesion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SesionExists(id))
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

        // DELETE: api/Sesiones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSesion(int id)
        {
            var sesion = await _context.Sesiones.FindAsync(id);
            if (sesion == null)
            {
                return NotFound();
            }

            // En lugar de eliminar, marcar como cancelada
            sesion.Estado = "Cancelada";
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/Sesiones/5/estado
        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] string estado)
        {
            var sesion = await _context.Sesiones.FindAsync(id);
            if (sesion == null)
            {
                return NotFound();
            }

            sesion.Estado = estado;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SesionExists(int id)
        {
            return _context.Sesiones.Any(e => e.Id == id);
        }
    }
}