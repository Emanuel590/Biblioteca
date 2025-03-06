using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiBiblioteca.Models
{
    public class Productos
    {
        [Key]
        public int Id_productos { get; set; }
        public string Nombre { get; set; }
        [ForeignKey("Categorias")]
        public int Id_categoria { get; set; }

        [ForeignKey("Estados")]
        public int ID_ESTADO { get; set; }



    }
}
