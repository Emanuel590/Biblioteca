using ApiBiblioteca.Data;
using ApiBiblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiBiblioteca.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {

        private readonly AplicationDbContext _context;

        public ProductosController(AplicationDbContext context, IConfiguration config)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Productos>>> GetProductos()
        {
            return await _context.BIBLIOTECA_PRODUCTOS_TB.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Productos>> GetProductos(int id)
        {
            var productos = await _context.BIBLIOTECA_PRODUCTOS_TB.FindAsync(id);

            if (productos == null)
            {

                return NotFound(
                    new
                    {
                        mensaje = "productos no encontrados"
                    }
                );
            }
            return productos;
        }


        [HttpPost]
        [Consumes("multipart/form-data")]

        public async Task<ActionResult<Productos>> AddProductos([FromForm] Productos productos)
        {
            string imgFolder = Path.Combine(Directory.GetCurrentDirectory(), "img");

            var extension = Path.GetExtension(productos.foto.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";

            string fullPath = Path.Combine(imgFolder, fileName);
            try
            {
                using (FileStream newFile = System.IO.File.Create(fullPath))
                {
                    await productos.foto.CopyToAsync(newFile);
                    await newFile.FlushAsync();
                }

                productos.foto = null;
                productos.FotoPath = $"https://localhost:7003/img/{fileName}";

                _context.BIBLIOTECA_PRODUCTOS_TB.Add(productos);
                await _context.SaveChangesAsync();


                return CreatedAtAction(
                    nameof(GetProductos),
                    new
                    {
                        mensaje = "produto creado",
                        id = productos.Id_productos
                    },
                    productos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Productos>> UpdateProductos(int id, Productos productos)
        {
            if (productos == null)
            {

                return BadRequest(new { mensaje = "No se encontró el producto" });
            }

                if (!_context.BIBLIOTECA_PRODUCTOS_TB.Any(i => i.Id_productos == id))
                {
                    return NotFound(new{mensaje = "el productos no ha sido encontrada" });
                }

            _context.Entry(productos).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al actualizar el producto" });

            }
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductos(int id)
        {
            var productos = await _context.BIBLIOTECA_PRODUCTOS_TB.FindAsync(id);

            if (productos == null)
            {

                return NotFound(
                    new
                    {
                        mensaje = "La Productos no se ha encontrado o ya ha sido eliminada"
                    }
                );
            }

            _context.BIBLIOTECA_PRODUCTOS_TB.Remove(productos);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}

