using System.ComponentModel.DataAnnotations;

namespace SistemaEmpleados.Models
{
    public partial class Empleado
    {
        public int EmpleadoId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "Debe seleccionar un departamento")]
        public int DepartamentoId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un cargo")]
        public int CargoId { get; set; }

        [Required(ErrorMessage = "Debe ingresar la fecha de inicio")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "Debe ingresar el salario")]
        [Range(0.01, 1000000, ErrorMessage = "El salario debe ser mayor que 0")]
        public decimal Salario { get; set; }

        public bool Estado { get; set; }

        // Relaciones
        public virtual Cargo? Cargo { get; set; }
        public virtual Departamento? Departamento { get; set; }

        public virtual ICollection<VacacionesDisponible> VacacionesDisponibles { get; set; } = new List<VacacionesDisponible>();
    }
}
