using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiBiblioteca.Data;
using ApiBiblioteca.Models;

namespace ApiBiblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly AplicationDbContext _context;
        public PagosController(AplicationDbContext context)
            => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetPagos()
        {
            var lista = await _context.BIBLIOTECA_METODO_PAGO_TB
                .Select(p => new {
                    p.Id_metodo,
                    p.Metodo_Pago,
                    p.Entidad_Bancaria,
                    N_Tarjeta = p.TarjetaEnmascarada,  
                    p.ID_ESTADO
                })
                .ToListAsync();

            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPagos(int id)
        {
            var p = await _context.BIBLIOTECA_METODO_PAGO_TB.FindAsync(id);
            if (p == null)
                return NotFound(new { mensaje = "Pago no encontrado" });

            var dto = new
            {
                p.Id_metodo,
                p.Metodo_Pago,
                p.Entidad_Bancaria,
                N_Tarjeta = p.TarjetaEnmascarada,
                p.ID_ESTADO
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> AddPagos(Pagos pagos)
        {
            _context.BIBLIOTECA_METODO_PAGO_TB.Add(pagos);
            await _context.SaveChangesAsync();

            var dto = new
            {
                pagos.Id_metodo,
                pagos.Metodo_Pago,
                pagos.Entidad_Bancaria,
                N_Tarjeta = pagos.TarjetaEnmascarada,
                pagos.ID_ESTADO
            };
            return CreatedAtAction(nameof(GetPagos), new { id = pagos.Id_metodo }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePagos(int id, Pagos pagos)
        {
            if (id != pagos.Id_metodo)
                return BadRequest(new { mensaje = "Los ID no coinciden" });

            _context.Entry(pagos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.BIBLIOTECA_METODO_PAGO_TB.Any(f => f.Id_metodo == id))
                    return NotFound(new { mensaje = "Método de pago no encontrado" });
                throw;
            }

            var dto = new
            {
                pagos.Id_metodo,
                pagos.Metodo_Pago,
                pagos.Entidad_Bancaria,
                N_Tarjeta = pagos.TarjetaEnmascarada,
                pagos.ID_ESTADO
            };
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePago(int id)
        {
            var pago = await _context.BIBLIOTECA_METODO_PAGO_TB.FindAsync(id);
            if (pago == null)
                return NotFound(new { mensaje = "Método de pago no encontrado o ya eliminado" });

            _context.BIBLIOTECA_METODO_PAGO_TB.Remove(pago);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
