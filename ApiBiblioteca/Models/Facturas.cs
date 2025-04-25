using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiBiblioteca.Models
{
    public class Facturas
    {
        [Key]
        public int id_factura { get; set; }

        [Required]
        public DateTime fecha_factura { get; set; }

        [Required]
        public decimal total { get; set; }

        [ForeignKey("Usuario")]
        public int id_usuario { get; set; }

        [ForeignKey("MetodoPago")]
        public int id_metodo { get; set; }

        [ForeignKey("Producto")]
        public int? id_producto { get; set; }

        [ForeignKey("Reserva")]
        public int? id_reservas { get; set; }

        [ForeignKey("Estado")]
        public int id_estado { get; set; }

        public Usuarios? Usuario { get; set; }
        public Pagos? MetodoPago { get; set; }
        public Productos? Producto { get; set; }
        public Reservas? Reserva { get; set; }
        public Estados? Estado { get; set; }
    }
}