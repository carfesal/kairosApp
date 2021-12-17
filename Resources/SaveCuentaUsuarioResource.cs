using System.ComponentModel.DataAnnotations;

namespace kairosApp.Resources
{
    public class SaveCuentaUsuarioResource
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Alias { get; set; }
        
    }
}
