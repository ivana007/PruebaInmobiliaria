using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class Pago
    {
        [Key]
        public int IdPago { get; set; }
        public DateTime FechaPago { get; set; }
        public int NumeroPago { get; set; }
        public decimal Importe { get; set; }
        public int IdContrato { get; set; }
        public Contrato contrato { get; set; }
    }
    
}
