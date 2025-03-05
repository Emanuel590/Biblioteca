using ApiBiblioteca.Data;
using ApiBiblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiBiblioteca.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public GenerosController(AplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Generos>>> GetGeneros()
        {
            return await _context.BIBLIOTECA_GENERO_TB.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Generos>> GetGeneros(int id)
        {
            var generos = await _context.BIBLIOTECA_GENERO_TB.FindAsync(id);

            if (generos == null)
            {
                return NotFound(
                    new
                    {

                        mensaje = "genero no encontrado"

                    }


                    );

            }
            return generos;
        }

        [HttpPost]
        public async Task<ActionResult<Generos>> AddGeneros(Generos generos)
        {
            _context.BIBLIOTECA_GENERO_TB.Add(generos);
            await _context.SaveChangesAsync();
            return
                CreatedAtAction(
                    nameof(GetGeneros),
                    new
                    {
                        id = generos.Id_Genero,
                    },
                    generos
                    );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Generos>> UpdateGeneros(int id, Generos generos)
        {
            if (generos == null)
            {
                return BadRequest();

            }

            _context.Entry(generos).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!_context.BIBLIOTECA_GENERO_TB.Any(i => i.Id_Genero == id))
                {
                    return NotFound
                        (
                            new
                            {
                                elmesaje = "El genero no ha sido encontrado"
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
        public async Task<ActionResult> DeleteGeneros(int id)
        {
            var generos = await _context.BIBLIOTECA_GENERO_TB.FindAsync(id);

            if (generos == null)
            {
                return NotFound(
                        new
                        {
                            elmensaje = "Los generos no se ha encontrado o ya se ha eliminado"
                        }
                    );
            }

            _context.BIBLIOTECA_GENERO_TB.Remove(generos);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
