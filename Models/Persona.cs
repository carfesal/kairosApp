using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kairosApp.Models
{
    public class Persona
    {
        public int Id { get; set; }
        public string Cedula { get; set; }  
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Unidad { get; set; }
        public string CorreoAlterno { get; set; }
        public string rol { get; set; }
        public CuentaUsuario CuentaUsuario;


    }
}
