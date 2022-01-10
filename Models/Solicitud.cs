﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kairosApp.Models
{
    public class Solicitud
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Dictionary<string, object> InfoSolicitud { get; set; }
        public int PersonaId { get; set; }
        public Persona Persona { get; set; }
    }
}
