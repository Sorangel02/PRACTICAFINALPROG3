using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaEmpleados.Models;

namespace SistemaEmpleados.Controllers
{
    public class VacacionesDisponiblesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VacacionesDisponiblesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VacacionesDisponibles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.VacacionesDisponibles.Include(v => v.Empleado);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: VacacionesDisponibles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacacionesDisponible = await _context.VacacionesDisponibles
                .Include(v => v.Empleado)
                .FirstOrDefaultAsync(m => m.VacacionesId == id);
            if (vacacionesDisponible == null)
            {
                return NotFound();
            }

            return View(vacacionesDisponible);
        }

        // GET: VacacionesDisponibles/Create
        public IActionResult Create()
        {
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "EmpleadoId", "Nombre");
            return View();
        }

        // POST: VacacionesDisponibles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpleadoId")] VacacionesDisponible vacacionesDisponible)
        {
            if (!ModelState.IsValid)
            {
                var empleado = await _context.Empleados.FindAsync(vacacionesDisponible.EmpleadoId);

                if (empleado == null)
                {
                    ModelState.AddModelError("EmpleadoId", "El empleado seleccionado no es válido.");
                    ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "EmpleadoId", "Nombre", vacacionesDisponible.EmpleadoId);
                    return View(vacacionesDisponible);
                }

                ModelState.Clear();

                TimeSpan tiempoEnEmpresa = DateTime.Now - empleado.FechaInicio;
                int aniosDeServicio = (int)Math.Floor(tiempoEnEmpresa.TotalDays / 365.25);

                int diasAsignados = 0;
                if (aniosDeServicio >= 1 && aniosDeServicio < 5)
                {
                    diasAsignados = 14;
                }
                else if (aniosDeServicio >= 5)
                {
                    diasAsignados = 16;
                }

                vacacionesDisponible.Anio = DateTime.Now.Year;
                vacacionesDisponible.DiasAsignados = diasAsignados;
                vacacionesDisponible.DiasTomados = 0;
                vacacionesDisponible.DiasRestantes = diasAsignados;

                if (TryValidateModel(vacacionesDisponible))
                {
                    _context.Add(vacacionesDisponible);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "✅ Vacaciones registradas correctamente.";
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "EmpleadoId", "Nombre", vacacionesDisponible.EmpleadoId);
            return View(vacacionesDisponible);
        }

        // GET: VacacionesDisponibles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacacionesDisponible = await _context.VacacionesDisponibles.FindAsync(id);
            if (vacacionesDisponible == null)
            {
                return NotFound();
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "EmpleadoId", "Nombre", vacacionesDisponible.EmpleadoId);
            return View(vacacionesDisponible);
        }

        // POST: VacacionesDisponibles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VacacionesId,EmpleadoId,Anio,DiasAsignados,DiasTomados,DiasRestantes")] VacacionesDisponible vacacionesDisponible)
        {
            if (id != vacacionesDisponible.VacacionesId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacacionesDisponible);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacacionesDisponibleExists(vacacionesDisponible.VacacionesId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "EmpleadoId", "Nombre", vacacionesDisponible.EmpleadoId);
            return View(vacacionesDisponible);
        }

        // GET: VacacionesDisponibles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacacionesDisponible = await _context.VacacionesDisponibles
                .Include(v => v.Empleado)
                .FirstOrDefaultAsync(m => m.VacacionesId == id);
            if (vacacionesDisponible == null)
            {
                return NotFound();
            }

            return View(vacacionesDisponible);
        }

        // POST: VacacionesDisponibles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vacacionesDisponible = await _context.VacacionesDisponibles.FindAsync(id);
            if (vacacionesDisponible != null)
            {
                _context.VacacionesDisponibles.Remove(vacacionesDisponible);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: VacacionesDisponibles/GetEmpleadoDetails/{id}
        [HttpGet]
        public async Task<IActionResult> GetEmpleadoDetails(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            TimeSpan tiempoEnEmpresa = DateTime.Now - empleado.FechaInicio;
            int aniosDeServicio = (int)Math.Floor(tiempoEnEmpresa.TotalDays / 365.25);

            int diasAsignados = 0;
            if (aniosDeServicio >= 1 && aniosDeServicio < 5)
            {
                diasAsignados = 14;
            }
            else if (aniosDeServicio >= 5)
            {
                diasAsignados = 16;
            }

            return Json(new { fechaInicio = empleado.FechaInicio.ToShortDateString(), aniosDeServicio = aniosDeServicio, diasAsignados = diasAsignados });
        }

        private bool VacacionesDisponibleExists(int id)
        {
            return _context.VacacionesDisponibles.Any(e => e.VacacionesId == id);
        }
    }
}