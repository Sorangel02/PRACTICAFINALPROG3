using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaEmpleados.Models;

public partial class VacacionesDisponible
{
    public int VacacionesId { get; set; }

    public int EmpleadoId { get; set; }
    [Display(Name = "Año")]
    public int Anio { get; set; }
    [Display(Name = "Días asignados")]
    public int DiasAsignados { get; set; }
    [Display(Name = "Días tomados")]
    public int DiasTomados { get; set; }
    [Display(Name = "Días restantes")]
    public int? DiasRestantes { get; set; }

    public virtual Empleado Empleado { get; set; } = null!;
}
