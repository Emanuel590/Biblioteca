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

        public ProductosController(AplicationDbContext context)
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
        public async Task<ActionResult<Productos>> AddProductos(Productos productos)
        {
            _context.BIBLIOTECA_PRODUCTOS_TB.Add(productos);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetProductos),
                new
                {
                    id = productos.Id_productos,
                },
                productos
            );
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Productos>> UpdateProductos(int id, Productos productos)
        {
            if (productos == null)
            {

                return BadRequest();
            }

            _context.Entry(productos).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                if (!_context.BIBLIOTECA_PRODUCTOS_TB.Any(i => i.Id_productos == id))
                {
                    return NotFound(
                        new
                        {
                            mensaje = "el productos no ha sido encontrada"
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

