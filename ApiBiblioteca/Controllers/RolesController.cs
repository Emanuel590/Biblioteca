using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiBiblioteca.Data;
using ApiBiblioteca.Models;

namespace ApiBiblioteca.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public RolesController(AplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Roles>>> GetRoles()
        {
            return await _context.BIBLIOTECA_ROLE_TB.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Roles>> GetRole(int id)
        {
            var role = await _context.BIBLIOTECA_ROLE_TB.FindAsync(id);

            if (role == null)
            {
                return NotFound(new { mensaje = "Rol no encontrado" });
            }

            return role;
        }

        [HttpPost]
        public async Task<ActionResult<Roles>> AddRole(Roles role)
        {
            _context.BIBLIOTECA_ROLE_TB.Add(role);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRole), new { id = role.id_role }, role);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Roles>> UpdateRole(int id, Roles role)
        {
            if (role == null)
            {
                return BadRequest();
            }

            _context.Entry(role).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!_context.BIBLIOTECA_ROLE_TB.Any(r => r.id_role == id))
                {
                    return NotFound(new { mensaje = "El rol no fue encontrado" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            var role = await _context.BIBLIOTECA_ROLE_TB.FindAsync(id);

            if (role == null)
            {
                return NotFound(new { mensaje = "El rol no se ha encontrado o ya fue eliminado" });
            }

            _context.BIBLIOTECA_ROLE_TB.Remove(role);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
