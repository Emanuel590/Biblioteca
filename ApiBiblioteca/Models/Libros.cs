using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiBiblioteca.Models
{
    public class Libros
    {

        [Key]
        public int Id_libro { get; set; }
        public int Stock {  get; set; }
        public decimal precio_alquiler { get; set; }
        [ForeignKey("Autores")]
        public int Id_Autor { get; set; }
        [ForeignKey("Generos")]
        public int Id_Genero { get; set; }
        [ForeignKey("Reseñas")]
        public int Id_Review { get; set; }
        [ForeignKey("Estados")]
        public int Id_Estado { get; set; }

    }
}
