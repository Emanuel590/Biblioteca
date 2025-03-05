using ApiBiblioteca.Data;
using ApiBiblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiBiblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly AplicationDbContext _context;
        public AutoresController(AplicationDbContext context)
        {
            _context = context;
        }



        //GET Obtener los Autores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Autores>>> ObtenerAutores()
        {
            return await _context.BIBLIOTECA_AUTOR_TB.ToListAsync();
        }

        //GET Obtener los Autores por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Autores>> ObtenerAutoresPorId(int id)
        {
            var Autor = await _context.BIBLIOTECA_AUTOR_TB.FindAsync(id);
            if (Autor == null)
            {
                return NotFound(
                    new
                    {
                        mensaje = "Autor no encontrado"
                    }
                    );
            }
            return Autor;
        }


        //Crear un nuevo Autores
        [HttpPost]
        public async Task<ActionResult<Autores>> AgregarAutores(Autores Autor)
        {
            _context.BIBLIOTECA_AUTOR_TB.Add(Autor);
            await _context.SaveChangesAsync();

            return
                CreatedAtAction(
                    nameof(ObtenerAutores),
                    new
                    {
                        mensaje = "Autor creado",
                        id = Autor.Id_Autor
                    },

                       Autor);

        }


        //Actualizar el Autor
        [HttpPut("{id}")]
        public async Task<ActionResult<Autores>> ActualizarAutores(int id, Autores Autor)
        {
            if (Autor == null)
            {
                return BadRequest(new { mensaje = "No se encontró el Autor" });
            }

            if (!_context.BIBLIOTECA_AUTOR_TB.Any(A => A.Id_Autor == id))
            {
                return NotFound(new { mensaje = "No se encontró el Autor" });
            }

            _context.Entry(Autor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Error interno al actualizar el Autor" });
            }

            return NoContent();
        }


        //Eliminar un Autor
        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarAutores(int id)
        {
            var Autores = await _context.BIBLIOTECA_AUTOR_TB.FindAsync(id);
            if (Autores == null)
            {
                return NotFound
                        (
                    new
                    {
                        elmensaje = "El Autor no se encuentra"
                    }
                        );
            }

            _context.BIBLIOTECA_AUTOR_TB.Remove(Autores);
            await _context.SaveChangesAsync();
            return NoContent();
        }




    }
}
