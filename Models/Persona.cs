using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace kairosApp.Models
{
    [Table("personas")]
    public class Persona
    {
        public int Id { get; set; }
        public string Identificacion { get; set; }  
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Unidad { get; set; }
        public string CorreoAlterno { get; set; }
        public string Rol { get; set; }
        public CuentaUsuario CuentaUsuario;
        public IList<Solicitud> Solicitudes { get; set; } = new List<Solicitud>();


    }
}
