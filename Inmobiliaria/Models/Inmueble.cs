using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class Inmueble
    {
        [Key]
        public int IdInmueble { get; set; }
        public string Direccion { get; set; }
        public string TipoImnueble { get; set; }
        public string Precio { get; set; }
        public string CantHambientes { get; set; }
        public string Uso { get; set; }
        public string Estado { get; set; }
        public Propietario IdPropietario { get; set; }
    }
}
