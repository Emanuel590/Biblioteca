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

                _context.BIBLIOTECA_PRODUCTOS_TB.Add(productos); //---
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

        //EDIT 
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]

        public async Task<ActionResult<Productos>> UpdateProductos(int id, [FromForm] Productos productos)
        {
            if (productos == null)
            {
                return BadRequest(new { mensaje = "No se encontró el producto" });
            }

            var productoExiste = await _context.BIBLIOTECA_PRODUCTOS_TB.FirstOrDefaultAsync(p => p.Id_productos == id);
                if (productoExiste == null)
                {
                    return NotFound(new{mensaje = "el productos no ha sido encontrada" });
                }

                if (productos.foto != null)
            {
                string imgFolder = Path.Combine(Directory.GetCurrentDirectory(), "img");
                var extension = Path.GetExtension(productos.foto.FileName);
                var fileName = $"{Guid.NewGuid()}{extension}";
                string fullPath = Path.Combine(imgFolder, fileName);

                using (FileStream newFile = System.IO.File.Create(fullPath))
                {
                    await productos.foto.CopyToAsync(newFile);
                    await newFile.FlushAsync();
                }
                productos.FotoPath = $"https://localhost:7003/img/{fileName}";
            }
            else
            {
                productos.FotoPath = productoExiste.FotoPath;
            }

            productos.Id_productos = id;

            _context.Entry(productoExiste).State = EntityState.Detached;

            _context.Entry(productoExiste).State = EntityState.Modified;

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

        //EDIT ESTADO 
        [HttpPut("estado/{id}")]
        public async Task<ActionResult<Productos>> ActualizarEstadoProducto(int id, int estado)
        {
            var productoExisteId = await _context.BIBLIOTECA_PRODUCTOS_TB.FindAsync(id);
            if (productoExisteId == null)
            {
                return NotFound();
            }

            productoExisteId.ID_ESTADO = estado;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return StatusCode(500, new { mensaje = "error interno al actualizar el id "});
            }
            return NoContent();
        }



        //ELIMINAR
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

