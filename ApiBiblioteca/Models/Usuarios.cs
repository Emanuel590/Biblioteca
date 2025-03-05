using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ApiBiblioteca.Models
{
    public class Usuarios
    {
        [Key]
        public int id_usuario { get; set; }

        public string nombre { get; set; }


        public string email { get; set; }

        public int? codigo_postal { get; set; }

  
        public int telefono { get; set; }

        public int cedula { get; set; }

        public string contra { get; set; }

        [ForeignKey("Role")]
        public int id_role { get; set; }

        [ForeignKey("Estado")]
        public int id_estado { get; set; }


    }
}
