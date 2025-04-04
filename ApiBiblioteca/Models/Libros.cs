using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiBiblioteca.Models
{
    public class Libros
    {

        [Key]
        public int Id_libro { get; set; }
        public string Titulo { get; set; }
        public int Stock {  get; set; }
        public decimal precio_alquiler { get; set; }
        [ForeignKey("Autores")]
        public int Id_Autor { get; set; }
        [ForeignKey("Generos")]
        public int Id_Genero { get; set; }
        [ForeignKey("Estados")]
        public int Id_Estado { get; set; }

        public string FotoPath { get; set; } = string.Empty;

        [NotMapped]
        [JsonIgnore]
        public IFormFile foto { get; set; }

    }
}
