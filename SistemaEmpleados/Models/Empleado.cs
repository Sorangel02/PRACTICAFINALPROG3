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

       
        public string TiempoEnEmpresa
        {
            get
            {
                var hoy = DateTime.Now;
                var diferencia = hoy - FechaInicio;
                int anios = (int)(diferencia.TotalDays / 365.25);
                int meses = (int)((diferencia.TotalDays % 365.25) / 30.44);
                return $"{anios} años y {meses} meses";
            }
        }

        // Deducciones de ley
        public decimal AFP => Salario * 0.0287m; // Tasa 2024
        public decimal ARS => Salario * 0.0304m; // Tasa 2024
        public decimal ISR => CalcularISR();

        // Método privado para calcular el ISR
        private decimal CalcularISR()
        {
            // Tasas y rangos del ISR de la DGII para 2024 (mensual)
            var salarioAnual = Salario * 12;
            decimal isrAnual = 0;

            if (salarioAnual <= 416220.00m)
            {
                isrAnual = 0;
            }
            else if (salarioAnual <= 624329.00m)
            {
                isrAnual = (salarioAnual - 416220.00m) * 0.15m;
            }
            else if (salarioAnual <= 867123.00m)
            {
                isrAnual = 31216.50m + ((salarioAnual - 624329.00m) * 0.20m);
            }
            else
            {
                isrAnual = 79776.50m + ((salarioAnual - 867123.00m) * 0.25m);
            }

            return isrAnual / 12;
        }

        // Relaciones
        public virtual Cargo? Cargo { get; set; }
        public virtual Departamento? Departamento { get; set; }

        public virtual ICollection<VacacionesDisponible> VacacionesDisponibles { get; set; } = new List<VacacionesDisponible>();
    }
}