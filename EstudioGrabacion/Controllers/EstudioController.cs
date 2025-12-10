using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EstudioGrabacion.Data;
using EstudioGrabacion.Models;
using System.Diagnostics;

namespace EstudioGrabacion.Controllers
{
    [Authorize]
    public class EstudioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstudioController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Ingenieros()
        {
            ViewData["Title"] = "Ingenieros de Sonido";
            return View();
        }

        public IActionResult Servicios()
        {
            ViewData["Title"] = "Servicios del Estudio";
            return View();
        }

        public async Task<IActionResult> Sesiones()
        {
            ViewData["Title"] = "Sesiones de Grabación";

            var sesiones = await _context.Sesiones
                .Include(s => s.Ingeniero)
                .Include(s => s.Servicio)
                .OrderByDescending(s => s.FechaHoraInicio)
                .ToListAsync();

            return View(sesiones);
        }

        public IActionResult Paquetes()
        {
            ViewData["Title"] = "Paquetes de Servicios";
            return View();
        }

        public IActionResult Reservar()
        {
            ViewData["Title"] = "Reservar Sesión";
            return View();
        }
    }
}