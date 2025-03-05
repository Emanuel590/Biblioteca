using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiBiblioteca.Models
{
    public class Categorias
    {
        [Key]
       public int Id_Categoria { get; set; }
       public string Nombre { get; set; }
       [ForeignKey("Estados")]
       public int Id_Estado { get; set; }
    }
}
