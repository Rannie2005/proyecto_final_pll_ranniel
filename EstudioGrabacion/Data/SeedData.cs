using EstudioGrabacion.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EstudioGrabacion.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            // Crear roles
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "Empleado", "Cliente" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Crear usuario admin
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var adminEmail = "admin@estudio.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Crear datos de prueba si no existen
            if (!context.Ingenieros.Any())
            {
                var ingenieros = new[]
                {
                    new Ingeniero
                    {
                        Nombre = "Carlos Martínez",
                        Especialidad = "Mezcla y Masterización",
                        TarifaPorHora = 50,
                        Disponible = true,
                        Email = "carlos@estudio.com",
                        Telefono = "555-100-2000"
                    },
                    new Ingeniero
                    {
                        Nombre = "Ana López",
                        Especialidad = "Producción Musical",
                        TarifaPorHora = 60,
                        Disponible = true,
                        Email = "ana@estudio.com",
                        Telefono = "555-100-2001"
                    },
                    new Ingeniero
                    {
                        Nombre = "Juan Pérez",
                        Especialidad = "Grabación de Voz",
                        TarifaPorHora = 40,
                        Disponible = false, // No disponible
                        Email = "juan@estudio.com",
                        Telefono = "555-100-2002"
                    }
                };
                context.Ingenieros.AddRange(ingenieros);
                await context.SaveChangesAsync();
            }

            if (!context.Servicios.Any())
            {
                var servicios = new[]
                {
                    new Servicio
                    {
                        Nombre = "Grabación de Voz",
                        Descripcion = "Sesión de grabación vocal profesional con equipo de alta gama",
                        Precio = 100,
                        DuracionHoras = 2,
                        Activo = true,
                        Categoria = "Grabación"
                    },
                    new Servicio
                    {
                        Nombre = "Mezcla de Audio",
                        Descripcion = "Mezcla profesional de pistas multi-canal",
                        Precio = 150,
                        DuracionHoras = 3,
                        Activo = true,
                        Categoria = "Post-Producción"
                    },
                    new Servicio
                    {
                        Nombre = "Masterización",
                        Descripcion = "Masterización final de proyecto con estándares de industria",
                        Precio = 200,
                        DuracionHoras = 2,
                        Activo = true,
                        Categoria = "Post-Producción"
                    },
                    new Servicio
                    {
                        Nombre = "Producción Musical",
                        Descripcion = "Producción completa de canción desde cero",
                        Precio = 300,
                        DuracionHoras = 4,
                        Activo = true,
                        Categoria = "Producción"
                    },
                    new Servicio
                    {
                        Nombre = "Edición de Audio",
                        Descripcion = "Edición y limpieza de audio grabado",
                        Precio = 80,
                        DuracionHoras = 1,
                        Activo = true,
                        Categoria = "Post-Producción"
                    }
                };
                context.Servicios.AddRange(servicios);
                await context.SaveChangesAsync();
            }

            if (!context.Paquetes.Any())
            {
                // Obtener servicios para crear paquetes
                var servicios = await context.Servicios.ToListAsync();

                var paquetes = new[]
                {
                    new Paquete
                    {
                        Nombre = "Paquete Básico",
                        Descripcion = "Perfecto para artistas independientes",
                        PrecioTotal = 250, // Normalmente sería 280 (100+80+100)
                        DuracionTotalHoras = 5,
                        Activo = true,
                        ServicioIds = servicios
                            .Where(s => s.Nombre.Contains("Grabación") || s.Nombre.Contains("Edición"))
                            .Select(s => s.Id)
                            .Take(2)
                            .ToList()
                    },
                    new Paquete
                    {
                        Nombre = "Paquete Premium",
                        Descripcion = "Todo incluido para producciones profesionales",
                        PrecioTotal = 500, // Normalmente sería 650 (300+150+200)
                        DuracionTotalHoras = 9,
                        Activo = true,
                        ServicioIds = servicios
                            .Where(s => !s.Nombre.Contains("Edición"))
                            .Select(s => s.Id)
                            .Take(4)
                            .ToList()
                    }
                };

                // Guardar paquetes primero
                context.Paquetes.AddRange(paquetes);
                await context.SaveChangesAsync();

                // Crear relaciones muchos a muchos
                foreach (var paquete in paquetes)
                {
                    if (paquete.ServicioIds != null)
                    {
                        foreach (var servicioId in paquete.ServicioIds)
                        {
                            context.ServicioPaquetes.Add(new ServicioPaquete
                            {
                                PaqueteId = paquete.Id,
                                ServicioId = servicioId
                            });
                        }
                    }
                }
                await context.SaveChangesAsync();
            }

            if (!context.Sesiones.Any())
            {
                var ingeniero = await context.Ingenieros.FirstAsync();
                var servicio = await context.Servicios.FirstAsync();

                var sesiones = new[]
                {
                    new Sesion
                    {
                        FechaHoraInicio = DateTime.Now.AddDays(1),
                        FechaHoraFin = DateTime.Now.AddDays(1).AddHours(2),
                        CostoTotal = 200,
                        Estado = "Pendiente",
                        NombreCliente = "Cliente Demo",
                        TelefonoCliente = "555-123-4567",
                        EmailCliente = "cliente@demo.com",
                        IngenieroId = ingeniero.Id,
                        ServicioId = servicio.Id
                    },
                    new Sesion
                    {
                        FechaHoraInicio = DateTime.Now.AddDays(2),
                        FechaHoraFin = DateTime.Now.AddDays(2).AddHours(3),
                        CostoTotal = 300,
                        Estado = "Confirmada",
                        NombreCliente = "Otro Cliente",
                        TelefonoCliente = "555-987-6543",
                        EmailCliente = "otro@cliente.com",
                        IngenieroId = ingeniero.Id,
                        ServicioId = servicio.Id,
                        Notas = "Traer material propio"
                    }
                };
                context.Sesiones.AddRange(sesiones);
                await context.SaveChangesAsync();
            }
        }
    }
}