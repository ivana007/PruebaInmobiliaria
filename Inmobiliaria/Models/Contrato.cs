using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class Contrato
    {
        [Key]
        public int IdContrato { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaBaja { get; set; }
        public int IdGarante { get; set; }
        public int IdPago { get; set; }
        public int IdInquilino { get; set; }
        public int IdInmueble { get; set; }
       
        public Garante garante { get; set; }
        public Pago pago { get; set; }
        public Inquilino inquilino { get; set; }
        public Inmueble inmueble { get; set; }
    }
}
