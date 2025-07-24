using System;
using System.Collections.Generic;

namespace SistemaEmpleados.Models;

public partial class VacacionesDisponible
{
    public int VacacionesId { get; set; }

    public int EmpleadoId { get; set; }

    public int Anio { get; set; }

    public int DiasAsignados { get; set; }

    public int DiasTomados { get; set; }

    public int? DiasRestantes { get; set; }

    public virtual Empleado Empleado { get; set; } = null!;
}
