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

        [EmailAddress]
        public string email { get; set; }

        public long? codigo_postal { get; set; }

  
        public long telefono { get; set; }

        public long cedula { get; set; }

        [Required]
        public string contra { get; set; }

        [ForeignKey("Role")]
        public int id_role { get; set; }

        [ForeignKey("Estado")]
        public int id_estado { get; set; }

        public ICollection<Facturas> Facturas { get; set; }

    }
}
