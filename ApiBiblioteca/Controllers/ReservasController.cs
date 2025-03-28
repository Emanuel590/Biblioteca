using ApiBiblioteca.Data;
using ApiBiblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiBiblioteca.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {

        private readonly AplicationDbContext _context;

        public ReservasController(AplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservas>>> GetReservas()
        {
            return await _context.BIBLIOTECA_RESERVAS_TB.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reservas>> GetReservas(int id)
        {
            var reservas = await _context.BIBLIOTECA_RESERVAS_TB.FindAsync(id);

            if (reservas == null)
            {

                return NotFound(
                    new
                    {
                        mensaje = "reserva no encontrada"
                    }
                );
            }
            return reservas;
        }


        [HttpPost]
        public async Task<ActionResult<Reservas>> AddReservas(Reservas reservas)
        {
            _context.BIBLIOTECA_RESERVAS_TB.Add(reservas);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetReservas),
                new
                {
                    id = reservas.Id_reservas,
                },
                reservas
            );
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Reservas>> UpdateReservas(int id, Reservas reservas)
        {
            if (reservas == null)
            {

                return BadRequest();
            }

            _context.Entry(reservas).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                if (!_context.BIBLIOTECA_RESERVAS_TB.Any(i => i.Id_reservas == id))
                {
                    return NotFound(
                        new
                        {
                            mensaje = "la reserva no ha sido encontrada"
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
        public async Task<ActionResult> DeleteReservas(int id)
        {
            var reservas = await _context.BIBLIOTECA_RESERVAS_TB.FindAsync(id);

            if (reservas == null)
            {

                return NotFound(
                    new
                    {
                        mensaje = "la reserva no se ha encontrado o ya ha sido eliminada"
                    }
                );
            }

            _context.BIBLIOTECA_RESERVAS_TB.Remove(reservas);
            await _context.SaveChangesAsync();
            return NoContent();
        }



    }
}
