using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiBiblioteca.Models
{
    public class Roles
    {
        [Key]
        public int id_role { get; set; }

        [Required]
        [StringLength(100)]
        public string descripcion { get; set; }

        [ForeignKey("Estado")]
        public int id_estado { get; set; }

        public virtual Estados estado { get; set; }
    }
}
