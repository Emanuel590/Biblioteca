using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiBiblioteca.Models
{
    public class Resenas
    {

        [Key]
        public int Id_Resena { get; set; }

        public string Resena { get; set; }

        [ForeignKey("Estados")]
        public int Id_Estado { get; set; }

        [ForeignKey("Libros")]
        public int Id_Libro { get; set; }


        [ForeignKey("Usuarios")]
        public int Id_Usuario { get; set; }


    }
}
