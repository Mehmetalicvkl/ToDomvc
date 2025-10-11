using Microsoft.AspNetCore.Mvc;
using ToDomvs.Models;
using ToDomvs.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ToDoMvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserModel user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exists = _context.Users.Any(u => u.Username == user.Username || u.Email == user.Email);
            if (exists != null)
                return BadRequest(new { message = "Username or Email already exists." });

            user.PasswordHash = HashPassword(user.PasswordHash);
            user.Projects = new List<ProjectModel>();
            user.AssignedTasks = new List<TaskItemModel>();

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully." });
        }

        [HttpGet("AllUsers")]
        public IActionResult GetAll()
        {
            var users = _context.Users.Select(u => new { u.Id, u.Username, u.Email }).ToList();
            return Ok(users);
        }
    }
}
