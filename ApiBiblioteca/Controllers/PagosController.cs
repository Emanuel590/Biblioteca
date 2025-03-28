using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiBiblioteca.Data;
using ApiBiblioteca.Models;


namespace ApiBiblioteca.Controllers
{

    [Route("/api/[controller]")]
    [ApiController]

    public class PagosController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public PagosController(AplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pagos>>> GetPagos()
        {
            return await _context.BIBLIOTECA_METODO_PAGO_TB.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Pagos>> GetPagos(int id)
        {
            var pago = await _context.BIBLIOTECA_METODO_PAGO_TB.FindAsync(id);

            if (pago == null)
            {
                return NotFound(new { mensaje = "Pago no encontrado" });
            }

            return pago;
        }



        [HttpPost]
        public async Task<ActionResult<Pagos>> AddPagos(Pagos pagos)
        {
            _context.BIBLIOTECA_METODO_PAGO_TB.Add(pagos);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPagos), new { id = pagos.Id_metodo }, pagos);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePagos(int id, Pagos pagos)
        {
            if (id != pagos.Id_metodo)
            {
                return BadRequest(new { mensaje = "Los ID no coinciden" });
            }

            _context.Entry(pagos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.BIBLIOTECA_METODO_PAGO_TB.Any(f => f.Id_metodo == id))
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
            var pago = await _context.BIBLIOTECA_METODO_PAGO_TB.FindAsync(id);

            if (pago == null)
            {
                return NotFound(new { mensaje = "Factura no encontrada o ya eliminada" });
            }

            _context.BIBLIOTECA_METODO_PAGO_TB.Remove(pago);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }




}

