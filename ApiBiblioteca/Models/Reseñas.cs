using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiBiblioteca.Models
{
    public class Reseñas
    {

        [Key]
        public int Id_Reseña { get; set; }

        public string Reseña { get; set; }

        [ForeignKey("Estados")]
        public int Id_Estado { get; set; }


        [ForeignKey("Usuarios")]
        public int Id_Usuario { get; set; }
    }
}
