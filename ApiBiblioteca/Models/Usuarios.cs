using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ApiBiblioteca.Models
{
    public class Usuarios
    {
        [Key]
        public int id_usuario { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string email { get; set; }

        public int? codigo_postal { get; set; }

        [Required]
        public int telefono { get; set; }

        [Required]
        public int cedula { get; set; }

        [Required]
        [StringLength(20)]
        public string contra { get; set; }

        [ForeignKey("Role")]
        public int id_role { get; set; }

        [ForeignKey("Estado")]
        public int id_estado { get; set; }

        public virtual Roles role { get; set; }
        public virtual Estados estado { get; set; }
    }
}
