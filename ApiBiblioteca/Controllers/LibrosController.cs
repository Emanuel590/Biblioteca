using ApiBiblioteca.Data;
using ApiBiblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiBiblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly AplicationDbContext _context;
        //Constructor de la clase
        public LibrosController(AplicationDbContext context, IConfiguration config)
        {
            _context = context;
        }

        //GET Obtener los Libros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libros>>> ObtenerLibros()
        {   
            return await _context.BIBLIOTECA_LIBROS_TB.ToListAsync();
        }

        //GET Obtener los libros por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Libros>> ObtenerLibrosPorId(int id)
        {
            var Libro =  await _context.BIBLIOTECA_LIBROS_TB.FindAsync(id);
            if (Libro == null) 
            {
                return NotFound(
                    new
                    {
                        mensaje = "Libro no encontrado"
                    }
                    );
            }
            return Libro;
        }


        //Crear un nuevo libro
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<Libros>> AgregarLibros([FromForm] Libros libro)
        {

            string imgFolder = Path.Combine(Directory.GetCurrentDirectory(), "img");

            var extension = Path.GetExtension(libro.foto.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";

            string fullPath = Path.Combine(imgFolder, fileName);

            try
            {

                using (FileStream newFile = System.IO.File.Create(fullPath))
                {
                    await libro.foto.CopyToAsync(newFile);
                    await newFile.FlushAsync();
                }


                libro.foto = null;
                libro.FotoPath = $"https://localhost:7003/img/{fileName}";

                _context.BIBLIOTECA_LIBROS_TB.Add(libro);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(ObtenerLibros),
                    new { mensaje = "Libro creado", id = libro.Id_libro },
                    libro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





        //Actualizar el Libro
        [HttpPut("{id}")]
        public async Task<ActionResult<Libros>> ActualizarLibros(int id, Libros libro)
        {
            if (libro == null)
            {
                return BadRequest(new { mensaje = "No se encontró el libro" });
            }

            if (!_context.BIBLIOTECA_LIBROS_TB.Any(l => l.Id_libro == id))
            {
                return NotFound(new { mensaje = "No se encontró el libro" });
            }

            _context.Entry(libro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Error interno al actualizar el libro" });
            }

            return NoContent();
        }


        //Eliminar un Libro
        [HttpDelete("{id}")]
        public async  Task<ActionResult> EliminarLibros(int id)
        {
            var libros = await _context.BIBLIOTECA_LIBROS_TB.FindAsync(id);
            if(libros == null)
            {
                return NotFound
                        (
                    new
                    {
                        elmensaje = "El libro no se encuentra"
                    }
                        );
            }

            _context.BIBLIOTECA_LIBROS_TB.Remove(libros);
            await _context.SaveChangesAsync();
            return NoContent();
        }






















    }
}
