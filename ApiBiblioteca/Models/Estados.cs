using System.ComponentModel.DataAnnotations;

namespace ApiBiblioteca.Models
{
    public class Estados
    {
        [Key]
        public int Id_Estado { get; set; }
        public string Descripcion {get; set; }
    }
}
