using ApiBiblioteca.Data;
using ApiBiblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiBiblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public EstadosController(AplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estados>>> ObtenerEstados()
        {
            return await _context.BIBLIOTECA_ESTADO_TB.ToListAsync();
        }

        //GET Obtener los Autores por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Estados>> ObtenerEstadosPorId(int id)
        {
            var Estado = await _context.BIBLIOTECA_ESTADO_TB.FindAsync(id);
            if (Estado == null)
            {
                return NotFound(
                    new
                    {
                        mensaje = "Estado no encontrado"
                    }
                    );
            }
            return Estado;
        }


        //Crear un nuevo Autores
        [HttpPost]
        public async Task<ActionResult<Estados>> AgregarEstados(Estados Estado)
        {
            _context.BIBLIOTECA_ESTADO_TB.Add(Estado);
            await _context.SaveChangesAsync();

            return
                CreatedAtAction(
                    nameof(ObtenerEstados),
                    new
                    {
                        mensaje = "Estado creado",
                        id = Estado.Id_Estado
                    },

                       Estado);

        }


        //Actualizar el Autor
        [HttpPut("{id}")]
        public async Task<ActionResult<Estados>> ActualizarEstados(int id, Estados Estado)
        {
            if (Estado == null)
            {
                return BadRequest(new { mensaje = "No se encontró el Estado" });
            }

            if (!_context.BIBLIOTECA_ESTADO_TB.Any(E => E.Id_Estado == id))
            {
                return NotFound(new { mensaje = "No se encontró el Estado" });
            }

            _context.Entry(Estado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Error interno al actualizar el Estado" });
            }

            return NoContent();
        }


        //Eliminar un Autor
        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarEstados(int id)
        {
            var Estados = await _context.BIBLIOTECA_ESTADO_TB.FindAsync(id);
            if (Estados == null)
            {
                return NotFound
                        (
                    new
                    {
                        elmensaje = "El Estados no se encuentra"
                    }
                        );
            }

            _context.BIBLIOTECA_ESTADO_TB.Remove(Estados);
            await _context.SaveChangesAsync();
            return NoContent();
        }




    


}
}
