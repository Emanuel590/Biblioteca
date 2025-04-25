using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiBiblioteca.Models
{
    public class Productos
    {
        [Key]
        public int Id_productos { get; set; }
        public int Stock { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioProducto { get; set; } 

        [ForeignKey("Categorias")]
        public int Id_categoria { get; set; }

        [ForeignKey("Estados")]
        public int ID_ESTADO { get; set; }
        public string FotoPath { get; set; } = string.Empty;

        [NotMapped]
        [JsonIgnore]
        public IFormFile? foto { get; set; }

        public ICollection<Facturas> Facturas { get; set; }

    }
}
