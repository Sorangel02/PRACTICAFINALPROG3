using System;
using System.Collections.Generic;

namespace SistemaEmpleados.Models;

public partial class Empleado
{
    public int EmpleadoId { get; set; }

    public string Nombre { get; set; } = null!;

    public int DepartamentoId { get; set; }

    public int CargoId { get; set; }

    public DateOnly FechaInicio { get; set; }

    public decimal Salario { get; set; }

    public bool Estado { get; set; }

    public virtual Cargo Cargo { get; set; } = null!;

    public virtual Departamento Departamento { get; set; } = null!;

    public virtual ICollection<VacacionesDisponible> VacacionesDisponibles { get; set; } = new List<VacacionesDisponible>();
}
