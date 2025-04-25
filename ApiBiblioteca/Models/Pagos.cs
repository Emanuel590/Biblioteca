using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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


        [BindNever]
        public ICollection<Facturas> Facturas { get; set; } = new List<Facturas>();

        [NotMapped]
        public string TarjetaEnmascarada
        {
            get
            {
                var tarjetaStr = N_Tarjeta.ToString();
                return tarjetaStr.Length >= 4
                    ? "**** **** **** " + tarjetaStr.Substring(tarjetaStr.Length - 4)
                    : "**** (inválida)";
            }
        }

    }

}