using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiBiblioteca.Data;
using ApiBiblioteca.Models;
using ApiBiblioteca.Services;  
using BCrypt.Net;

namespace ApiBiblioteca.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AplicationDbContext _context;
        private readonly JwtService _jwtService; // servicio JWT

        public UsuariosController(AplicationDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuarios>>> GetUsuarios()
        {
            return await _context.BIBLIOTECA_USUARIOS_TB.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuarios>> GetUsuario(int id)
        {
            var usuario = await _context.BIBLIOTECA_USUARIOS_TB.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            return usuario;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Usuarios>> AddUsuario(Usuarios usuario)
        {
            var existingUser = await _context.BIBLIOTECA_USUARIOS_TB
                .FirstOrDefaultAsync(u => u.email == usuario.email);
            if (existingUser != null)
            {
                return BadRequest(new { mensaje = "El correo electrónico ya está registrado" });
            }

            usuario.contra = BCrypt.Net.BCrypt.HashPassword(usuario.contra);

            _context.BIBLIOTECA_USUARIOS_TB.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.id_usuario }, usuario);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginModel loginModel)
        {
            var usuario = await _context.BIBLIOTECA_USUARIOS_TB
                .FirstOrDefaultAsync(u => u.email == loginModel.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginModel.Contra, usuario.contra))
            {
                return Unauthorized(new { mensaje = "Correo electrónico o contraseña incorrectos" });
            }

            // Generamos el token JWT
            var token = _jwtService.GenerateToken(usuario.id_usuario, usuario.email, "Usuario");

            return Ok(new
            {
                mensaje = "Inicio de sesión exitoso",
                usuario = new { usuario.id_usuario, usuario.email },
                token
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Usuarios>> UpdateUsuario(int id, Usuarios usuario)
        {
            if (usuario == null)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!_context.BIBLIOTECA_USUARIOS_TB.Any(u => u.id_usuario == id))
                {
                    return NotFound(new { mensaje = "El usuario no fue encontrado" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.BIBLIOTECA_USUARIOS_TB.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(new { mensaje = "El usuario no se ha encontrado o ya fue eliminado" });
            }

            _context.BIBLIOTECA_USUARIOS_TB.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Contra { get; set; }
    }
}
