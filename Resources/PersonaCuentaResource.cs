using kairosApp.Models;

namespace kairosApp.Resources
{
    public class PersonaCuentaResource
    {
        public Persona Persona { get; set; }
        public IList<string> Usuarios { get; set; }
    }
}
