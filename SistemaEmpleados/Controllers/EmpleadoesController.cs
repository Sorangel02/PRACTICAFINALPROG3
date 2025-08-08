using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaEmpleados.Models;
using System.Text;
using System.Globalization; 


namespace SistemaEmpleados.Controllers
{
    public class EmpleadoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpleadoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Empleadoes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Empleados
                .Include(e => e.Cargo)
                .Include(e => e.Departamento);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Empleadoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var empleado = await _context.Empleados
                .Include(e => e.Cargo)
                .Include(e => e.Departamento)
                .FirstOrDefaultAsync(m => m.EmpleadoId == id);
            if (empleado == null) return NotFound();

            return View(empleado);
        }

        // GET: Empleadoes/Create
        public IActionResult Create()
        {
            ViewData["CargoId"] = new SelectList(_context.Cargos, "CargoId", "Nombre");
            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "DepartamentoId", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleado empleado)
        {
            bool idExiste = await _context.Empleados.AnyAsync(e => e.EmpleadoId == empleado.EmpleadoId);

            if (idExiste)
            {
                ModelState.AddModelError("EmpleadoId", "Ya existe un empleado con ese código. Por favor, ingrese otro.");

                ViewData["CargoId"] = new SelectList(_context.Cargos, "CargoId", "Nombre", empleado.CargoId);
                ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "DepartamentoId", "Nombre", empleado.DepartamentoId);

                return View(empleado);
            }

            if (ModelState.IsValid)
            {
                _context.Add(empleado);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "✅ Empleado creado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["CargoId"] = new SelectList(_context.Cargos, "CargoId", "Nombre", empleado.CargoId);
            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "DepartamentoId", "Nombre", empleado.DepartamentoId);

            return View(empleado);
        }

        // GET: Empleadoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();

            ViewData["CargoId"] = new SelectList(_context.Cargos, "CargoId", "Nombre", empleado.CargoId);
            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "DepartamentoId", "Nombre", empleado.DepartamentoId);
            return View(empleado);
        }

        // POST: Empleadoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmpleadoId,Nombre,DepartamentoId,CargoId,FechaInicio,Salario,Estado")] Empleado empleado)
        {
            if (id != empleado.EmpleadoId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empleado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.EmpleadoId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CargoId"] = new SelectList(_context.Cargos, "CargoId", "Nombre", empleado.CargoId);
            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "DepartamentoId", "Nombre", empleado.DepartamentoId);
            return View(empleado);
        }

        // GET: Empleadoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var empleado = await _context.Empleados
                .Include(e => e.Cargo)
                .Include(e => e.Departamento)
                .FirstOrDefaultAsync(m => m.EmpleadoId == id);
            if (empleado == null) return NotFound();

            return View(empleado);
        }

        // POST: Empleadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
            return _context.Empleados.Any(e => e.EmpleadoId == id);
        }

        // Exportar a CSV
        public async Task<IActionResult> ExportarEmpleadosACsv()
        {
            var empleados = await _context.Empleados
                .Include(e => e.Departamento)
                .Include(e => e.Cargo)
                .ToListAsync();

            var sb = new StringBuilder();

           
            string delimitador = ";";

          
            sb.AppendLine($"ID{delimitador}Nombre{delimitador}Departamento{delimitador}Cargo{delimitador}FechaInicio{delimitador}Salario{delimitador}Estado{delimitador}TiempoEnEmpresa{delimitador}AFP{delimitador}ARS{delimitador}ISR");

            // Datos de los empleados
            foreach (var empleado in empleados)
            {
                
                var culturaInvariante = CultureInfo.InvariantCulture;

                var linea = $"{empleado.EmpleadoId}{delimitador}" +
                            $"{empleado.Nombre.Replace(";", "")}{delimitador}" +
                            $"{empleado.Departamento.Nombre.Replace(";", "")}{delimitador}" +
                            $"{empleado.Cargo.Nombre.Replace(";", "")}{delimitador}" +
                            $"{empleado.FechaInicio.ToShortDateString()}{delimitador}" +
                            $"{empleado.Salario.ToString(culturaInvariante)}{delimitador}" +
                            $"{empleado.Estado}{delimitador}" +
                            $"{empleado.TiempoEnEmpresa.Replace(";", "")}{delimitador}" +
                            $"{empleado.AFP.ToString(culturaInvariante)}{delimitador}" +
                            $"{empleado.ARS.ToString(culturaInvariante)}{delimitador}" +
                            $"{empleado.ISR.ToString(culturaInvariante)}";

                sb.AppendLine(linea);
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Empleados.csv");
        }
    }
}

