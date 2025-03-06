using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiBiblioteca.Models
{
    public class Reservas
    {
        [Key]
        public int Id_reservas{ get; set; }
        [ForeignKey("Libros")]
        public int Id_Libro { get; set; }

        [ForeignKey("Usuarios")]
        public int Id_Usuario { get; set; }

        public DateTime Fecha { get; set; }

        [ForeignKey("Estados")]
        public int ID_ESTADO { get; set; }

    }
}
