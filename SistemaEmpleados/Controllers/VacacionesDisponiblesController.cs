using System;
using System.Collections.Generic;
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
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "EmpleadoId", "EmpleadoId");
            return View();
        }

        // POST: VacacionesDisponibles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VacacionesId,EmpleadoId,Anio,Dias Asignados,Dias Tomados,Dias Restantes")] VacacionesDisponible vacacionesDisponible)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vacacionesDisponible);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "EmpleadoId", "EmpleadoId", vacacionesDisponible.EmpleadoId);
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
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "EmpleadoId", "EmpleadoId", vacacionesDisponible.EmpleadoId);
            return View(vacacionesDisponible);
        }

        // POST: VacacionesDisponibles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VacacionesId,EmpleadoId,Anio,Dias Asignados,Dias Tomados,Dias Restantes")] VacacionesDisponible vacacionesDisponible)
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
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "EmpleadoId", "EmpleadoId", vacacionesDisponible.EmpleadoId);
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

        private bool VacacionesDisponibleExists(int id)
        {
            return _context.VacacionesDisponibles.Any(e => e.VacacionesId == id);
        }
    }
}
