using ApiBiblioteca.Data;
using ApiBiblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiBiblioteca.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public CategoriasController (AplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categorias>>> ObtenerCategorias()
        {
            return await _context.BIBLIOTECA_CATEGORIA_TB.ToListAsync();
        }

        //GET Obtener los Autores por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Categorias>> ObtenerCategoriasPorId(int id)
        {
            var Categoria = await _context.BIBLIOTECA_CATEGORIA_TB.FindAsync(id);
            if (Categoria == null)
            {
                return NotFound(
                    new
                    {
                        mensaje = "Categoria no encontrado"
                    }
                    );
            }
            return Categoria;
        }


        //Crear un nuevo Autores
        [HttpPost]
        public async Task<ActionResult<Categorias>> AgregarCategorias(Categorias Categoria)
        {
            _context.BIBLIOTECA_CATEGORIA_TB.Add(Categoria);
            await _context.SaveChangesAsync();

            return
                CreatedAtAction(
                    nameof(ObtenerCategorias),
                    new
                    {
                        mensaje = "Estado creado",
                        id = Categoria.Id_Estado
                    },

                       Categoria);

        }


        //Actualizar el Autor
        [HttpPut("{id}")]
        public async Task<ActionResult<Categorias>> ActualizarEstados(int id, Categorias Categoria)
        {
            if (Categoria == null)
            {
                return BadRequest(new { mensaje = "No se encontró el Categoria" });
            }

            if (!_context.BIBLIOTECA_CATEGORIA_TB.Any(C => C.Id_Categoria == id))
            {
                return NotFound(new { mensaje = "No se encontró el Categoria" });
            }

            _context.Entry(Categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Error interno al actualizar el Categoria" });
            }

            return NoContent();
        }


        //Eliminar un Autor
        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarCategorias(int id)
        {
            var Categorias = await _context.BIBLIOTECA_CATEGORIA_TB.FindAsync(id);
            if (Categorias == null)
            {
                return NotFound
                        (
                    new
                    {
                        elmensaje = "El Categorias no se encuentra"
                    }
                        );
            }

            _context.BIBLIOTECA_CATEGORIA_TB.Remove(Categorias);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
