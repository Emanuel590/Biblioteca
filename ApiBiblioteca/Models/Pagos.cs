using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiBiblioteca.Models
{
    public class Pagos
    {
        [Key]
        public int Id_metodo { get; set; }
        public string Metodo_Pago { get; set; }
        public string Entidad_Bancaria { get; set; }
        public long N_Tarjeta { get; set; }

        [ForeignKey("Estados")]
        public int ID_ESTADO { get; set; }



    }
}
