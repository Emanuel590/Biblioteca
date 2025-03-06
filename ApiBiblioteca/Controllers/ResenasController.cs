using ApiBiblioteca.Data;
using ApiBiblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiBiblioteca.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ResenasController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public ResenasController(AplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todas las reseñas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resenas>>> GetResenas()
        {
            return await _context.BIBLIOTECA_RESENA_TB.ToListAsync();
        }

        // Obtener una reseña específica por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Resenas>> GetResenas(int id)
        {
            var resenas = await _context.BIBLIOTECA_RESENA_TB.FindAsync(id);

            if (resenas == null)
            {
                // Si no se encuentra la reseña, retornar un mensaje de error
                return NotFound(
                    new
                    {
                        mensaje = "Reseña no encontrada"
                    }
                );
            }
            return resenas;
        }

        // Agregar una nueva reseña
        [HttpPost]
        public async Task<ActionResult<Resenas>> AddResenas(Resenas resenas)
        {
            _context.BIBLIOTECA_RESENA_TB.Add(resenas);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetResenas),
                new
                {
                    id = resenas.Id_Resena,
                },
                resenas
            );
        }

        // Actualizar una reseña existente
        [HttpPut("{id}")]
        public async Task<ActionResult<Resenas>> UpdateResenas(int id, Resenas resenas)
        {
            if (resenas == null)
            {
                // Si la reseña no es válida, retornar un error
                return BadRequest();
            }

            _context.Entry(resenas).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Si no se encuentra la reseña en la base de datos, retornar un mensaje de error
                if (!_context.BIBLIOTECA_RESENA_TB.Any(i => i.Id_Resena == id))
                {
                    return NotFound(
                        new
                        {
                            mensaje = "La reseña no ha sido encontrada"
                        }
                    );
                }
                else
                {
                    // Si ocurre otro error, lanzar una excepción
                    throw;
                }
            }
            return NoContent();
        }

        // Eliminar una reseña por ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteResenas(int id)
        {
            var resenas = await _context.BIBLIOTECA_RESENA_TB.FindAsync(id);

            if (resenas == null)
            {
                // Si no se encuentra la reseña, retornar un mensaje de error
                return NotFound(
                    new
                    {
                        mensaje = "La reseña no se ha encontrado o ya ha sido eliminada"
                    }
                );
            }

            _context.BIBLIOTECA_RESENA_TB.Remove(resenas);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
