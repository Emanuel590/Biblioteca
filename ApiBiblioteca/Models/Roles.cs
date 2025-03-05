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
        public int Id_estado { get; set; }

    }
}
