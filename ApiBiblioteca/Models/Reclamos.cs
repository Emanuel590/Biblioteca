using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiBiblioteca.Models
{
    public class Reclamos
    {
        [Key]
        public int Id_Reclamo { get; set; }

        [ForeignKey("Libros")]
        public int Id_Libro { get; set; }

        [ForeignKey("Usuarios")]
        public int Id_Usuario { get; set; }

        public string Reclamo { get; set; }

        [ForeignKey("Estados")]
        public int Id_Estado { get; set; }

    }
}

