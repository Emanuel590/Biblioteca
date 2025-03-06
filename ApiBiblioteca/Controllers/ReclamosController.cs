using ApiBiblioteca.Data;
using ApiBiblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiBiblioteca.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ReclamosController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public ReclamosController(AplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reclamos>>> GetReclamos()
        {
            return await _context.BIBLIOTECA_RECLAMOS_TB.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reclamos>> GetReclamos(int id)
        {
            var reclamos = await _context.BIBLIOTECA_RECLAMOS_TB.FindAsync(id);

            if (reclamos == null)
            {
                return NotFound(
                    new
                    {
                        mensaje = "reclamo no encontrado"
                    }
                );
            }
            return reclamos;
        }

        [HttpPost]
        public async Task<ActionResult<Reclamos>> AddReclamos(Reclamos reclamos)
        {
            _context.BIBLIOTECA_RECLAMOS_TB.Add(reclamos);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetReclamos),
                new
                {
                    id = reclamos.Id_Reclamo,
                },
                reclamos
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Reclamos>> UpdateReclamos(int id, Reclamos reclamos)
        {
            if (reclamos == null)
            {
                return BadRequest();
            }

            _context.Entry(reclamos).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!_context.BIBLIOTECA_RECLAMOS_TB.Any(i => i.Id_Reclamo == id))
                {
                    return NotFound(
                        new
                        {
                            elmensaje = "El reclamo no ha sido encontrado"
                        }
                    );
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReclamos(int id)
        {
            var reclamos = await _context.BIBLIOTECA_RECLAMOS_TB.FindAsync(id);

            if (reclamos == null)
            {
                return NotFound(
                    new
                    {
                        elmensaje = "Los reclamos no se han encontrado o ya se han eliminado"
                    }
                );
            }

            _context.BIBLIOTECA_RECLAMOS_TB.Remove(reclamos);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
