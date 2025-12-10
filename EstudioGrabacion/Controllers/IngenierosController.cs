using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EstudioGrabacion.Data;
using EstudioGrabacion.Models;

namespace EstudioGrabacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngenierosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IngenierosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Ingenieros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingeniero>>> GetIngenieros()
        {
            return await _context.Ingenieros.ToListAsync();
        }

        // GET: api/Ingenieros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ingeniero>> GetIngeniero(int id)
        {
            var ingeniero = await _context.Ingenieros.FindAsync(id);

            if (ingeniero == null)
            {
                return NotFound();
            }

            return ingeniero;
        }

        // PUT: api/Ingenieros/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngeniero(int id, Ingeniero ingeniero)
        {
            if (id != ingeniero.Id)
            {
                return BadRequest();
            }

            _context.Entry(ingeniero).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngenieroExists(id))
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

        // POST: api/Ingenieros
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ingeniero>> PostIngeniero(Ingeniero ingeniero)
        {
            _context.Ingenieros.Add(ingeniero);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIngeniero", new { id = ingeniero.Id }, ingeniero);
        }

        // DELETE: api/Ingenieros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngeniero(int id)
        {
            var ingeniero = await _context.Ingenieros.FindAsync(id);
            if (ingeniero == null)
            {
                return NotFound();
            }

            _context.Ingenieros.Remove(ingeniero);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IngenieroExists(int id)
        {
            return _context.Ingenieros.Any(e => e.Id == id);
        }
    }
}
