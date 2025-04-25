using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiBiblioteca.Data;
using ApiBiblioteca.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiBiblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public PagosController(AplicationDbContext context)
            => _context = context;

        // Obtener el id_usuario desde el token de autenticación
        private int GetUserIdFromToken()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier); // El id_usuario puede ser almacenado en el token como NameIdentifier
            return claim != null ? int.Parse(claim.Value) : 0;
        }

        // Endpoint modificado para filtrar por id_usuario desde el token
        [HttpGet]
        public async Task<IActionResult> GetPagos()
        {
            int idUsuario = GetUserIdFromToken(); // Obtener el id_usuario del token

            if (idUsuario == 0)
            {
                return Unauthorized(new { mensaje = "No autorizado. El usuario no está autenticado." });
            }

            // Filtrar pagos por id_usuario del token
            var lista = await _context.BIBLIOTECA_METODO_PAGO_TB
                .Where(p => p.Id_usuario == idUsuario)
                .Select(p => new
                {
                    p.Id_metodo,
                    p.Metodo_Pago,
                    p.Entidad_Bancaria,
                    N_Tarjeta = p.TarjetaEnmascarada,
                    p.ID_ESTADO,
                    p.Id_usuario
                })
                .ToListAsync();

            if (lista.Count == 0)
            {
                return NotFound(new { mensaje = "No se encontraron pagos para este usuario" });
            }

            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPago(int id)
        {
            var p = await _context.BIBLIOTECA_METODO_PAGO_TB.FindAsync(id);
            if (p == null)
                return NotFound(new { mensaje = "Pago no encontrado" });

            var dto = new
            {
                p.Id_metodo,
                p.Metodo_Pago,
                p.Entidad_Bancaria,
                N_Tarjeta = p.TarjetaEnmascarada,
                p.ID_ESTADO,
                p.Id_usuario
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> AddPago(Pagos pagos)
        {
            // Verificar que Id_usuario esté asignado correctamente
            if (pagos.Id_usuario == 0)
            {
                return BadRequest(new { mensaje = "El campo Id_usuario es obligatorio." });
            }

            _context.BIBLIOTECA_METODO_PAGO_TB.Add(pagos);
            await _context.SaveChangesAsync();

            var dto = new
            {
                pagos.Id_metodo,
                pagos.Metodo_Pago,
                pagos.Entidad_Bancaria,
                N_Tarjeta = pagos.TarjetaEnmascarada,
                pagos.ID_ESTADO,
                pagos.Id_usuario
            };
            return CreatedAtAction(nameof(GetPago), new { id = pagos.Id_metodo }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePago(int id, Pagos pagos)
        {
            if (id != pagos.Id_metodo)
                return BadRequest(new { mensaje = "Los ID no coinciden" });

            // Verificar que Id_usuario esté asignado correctamente antes de la actualización
            if (pagos.Id_usuario == 0)
            {
                return BadRequest(new { mensaje = "El campo Id_usuario es obligatorio." });
            }

            _context.Entry(pagos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.BIBLIOTECA_METODO_PAGO_TB.Any(f => f.Id_metodo == id))
                    return NotFound(new { mensaje = "Método de pago no encontrado" });
                throw;
            }

            var dto = new
            {
                pagos.Id_metodo,
                pagos.Metodo_Pago,
                pagos.Entidad_Bancaria,
                N_Tarjeta = pagos.TarjetaEnmascarada,
                pagos.ID_ESTADO,
                pagos.Id_usuario
            };
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePago(int id)
        {
            var pago = await _context.BIBLIOTECA_METODO_PAGO_TB.FindAsync(id);
            if (pago == null)
                return NotFound(new { mensaje = "Método de pago no encontrado o ya eliminado" });

            _context.BIBLIOTECA_METODO_PAGO_TB.Remove(pago);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
