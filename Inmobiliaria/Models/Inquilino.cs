using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class Inquilino
    {
        [Key]
        public int IdInquilino { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public string Telefono { get; set; }
        public string Mail { get; set; }
        public string LugarTrabajo { get; set; }
        public string Condicion { get; set; }
    }
}
