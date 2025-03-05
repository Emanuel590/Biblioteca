using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiBiblioteca.Models
{
    public class Autores
    {
        [Key]
       public int Id_Autor {  get; set; }
       public string Nombre { get; set; }
       public string Apellido { get; set; }

        [ForeignKey("Estados")]
       public int Id_Estado { get; set; }
    }
}
