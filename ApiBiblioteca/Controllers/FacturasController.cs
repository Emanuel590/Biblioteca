using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiBiblioteca.Data;
using ApiBiblioteca.Models;

namespace ApiBiblioteca.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class FacturasController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public FacturasController(AplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Facturas>>> GetFacturas()
        {
            return await _context.BIBLIOTECA_FACTURAS_TB.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Facturas>> GetFactura(int id)
        {
            var factura = await _context.BIBLIOTECA_FACTURAS_TB.FindAsync(id);

            if (factura == null)
            {
                return NotFound(new { mensaje = "Factura no encontrada" });
            }

            return factura;
        }

        [HttpPost]
        public async Task<ActionResult<Facturas>> AddFactura(Facturas factura)
        {
            _context.BIBLIOTECA_FACTURAS_TB.Add(factura);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFactura), new { id = factura.id_factura }, factura);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFactura(int id, Facturas factura)
        {
            if (id != factura.id_factura)
            {
                return BadRequest(new { mensaje = "Los ID no coinciden" });
            }

            _context.Entry(factura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.BIBLIOTECA_FACTURAS_TB.Any(f => f.id_factura == id))
                {
                    return NotFound(new { mensaje = "Factura no encontrada" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFactura(int id)
        {
            var factura = await _context.BIBLIOTECA_FACTURAS_TB.FindAsync(id);

            if (factura == null)
            {
                return NotFound(new { mensaje = "Factura no encontrada o ya eliminada" });
            }

            _context.BIBLIOTECA_FACTURAS_TB.Remove(factura);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("admin/facturas")]
        public IActionResult ObtenerFacturas()
        {
            var facturas = _context.BIBLIOTECA_FACTURAS_TB
                .Include(f => f.Usuario)  
                .Include(f => f.MetodoPago)  
                .Include(f => f.Producto)  
                .Include(f => f.Reserva)  
                .Include(f => f.Estado)  
                .Select(f => new {
                    f.id_factura,
                    f.fecha_factura,
                    f.total,
                    USUARIO = f.Usuario.nombre,
                    PRODUCTO = f.Producto.Nombre,
                    METODO_PAGO = f.MetodoPago.Metodo_Pago,
                    RESERVA = f.Reserva.Id_reservas,
                    ESTADO = f.Estado.Descripcion,  
                }).ToList();

            return Ok(facturas);
        }

        [HttpGet("usuarios")]
        public IActionResult GetUsuarios()
        {
            var usuarios = _context.BIBLIOTECA_USUARIOS_TB.ToList(); 
            return Ok(usuarios);
        }

        [HttpGet("productos")]
        public IActionResult GetProductos()
        {
            var productos = _context.BIBLIOTECA_PRODUCTOS_TB.ToList();  
            return Ok(productos);
        }

        [HttpGet("metodosPago")]
        public IActionResult GetMetodosPago()
        {
            var metodosPago = _context.BIBLIOTECA_METODO_PAGO_TB.ToList();  
            return Ok(metodosPago);
        }

        [HttpGet("estados")]
        public IActionResult GetEstados()
        {
            var estados = _context.BIBLIOTECA_ESTADO_TB.ToList();  
            return Ok(estados);
        }
        [HttpGet("reserva")]

        public IActionResult GetReserva()
        {
            var reserva = _context.BIBLIOTECA_RESERVAS_TB.ToList();
            return Ok(reserva);
        }

    }
}
