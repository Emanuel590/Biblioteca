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


        [HttpGet]
        [Route("MayorAMenor")]
        public async Task<ActionResult<IEnumerable<Libros>>> OrdenarMayorAMenor()
        {
            var librosMayorMenor = await _context.BIBLIOTECA_LIBROS_TB
                .OrderByDescending(l => l.precio_alquiler)
                .ToListAsync();

            return Ok(librosMayorMenor);
        }


        [HttpGet]
        [Route("MenorAMayor")]
        public async Task<ActionResult<IEnumerable<Libros>>> OrdenarMenorAMayor()
        {
            var librosMenorMayor = await _context.BIBLIOTECA_LIBROS_TB
                .OrderBy(l => l.precio_alquiler)
                .ToListAsync();

            return Ok(librosMenorMayor);
        }


        [HttpGet("Genero/{idGenero}")]
        public async Task<ActionResult<IEnumerable<Libros>>> ObtenerLibroGenero(int idGenero)
        {
            var librosGenero = await _context.BIBLIOTECA_LIBROS_TB
                .Where(g => g.Id_Genero == idGenero)
                .ToListAsync();

            if (librosGenero == null || !librosGenero.Any())
                return NotFound("No se encontraron libros con ese género.");

            return Ok(librosGenero);
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
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<Libros>> ActualizarLibros(int id, [FromForm] Libros libro)
        {
            if (libro == null)
            {
                return BadRequest(new { mensaje = "No se encontró el libro" });
            }


            var libroExistente = await _context.BIBLIOTECA_LIBROS_TB.FirstOrDefaultAsync(l => l.Id_libro == id);
            if (libroExistente == null)
            {
                return NotFound(new { mensaje = "No se encontró el libro" });
            }


            if (libro.foto != null)
            {
                string imgFolder = Path.Combine(Directory.GetCurrentDirectory(), "img");
                var extension = Path.GetExtension(libro.foto.FileName);
                var fileName = $"{Guid.NewGuid()}{extension}";
                string fullPath = Path.Combine(imgFolder, fileName);

                using (FileStream newFile = System.IO.File.Create(fullPath))
                {
                    await libro.foto.CopyToAsync(newFile);
                    await newFile.FlushAsync();
                }
                libro.FotoPath = $"https://localhost:7003/img/{fileName}";
            }
            else
            {
                libro.FotoPath = libroExistente.FotoPath;
            }


            libro.Id_libro = id;

            // Esto lo que hace es desanexar la entidad existente para evitar conflictos de tracking con el id
            _context.Entry(libroExistente).State = EntityState.Detached;

            // Ahora, adjuntar la entidad entrante como modificada
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


        [HttpPut("estado/{id}")]
        public async Task<ActionResult<Libros>> ActualizarEstadoLibro(int id, int estado)
        {
            var libroExistente = await _context.BIBLIOTECA_LIBROS_TB.FindAsync(id);
            if(libroExistente == null)
            {
                return NotFound();
            }

            libroExistente.Id_Estado = estado;
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
