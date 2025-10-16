using Microsoft.AspNetCore.Mvc;
using ToDomvs.Data;
using ToDomvs.Models;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ToDomvs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UserApiController(AppDbContext context)
        {
            _context = context;
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
            if (exists)
                return BadRequest(new { success = false, message = "Username or Email already exists." });

            user.PasswordHash = HashPassword(user.PasswordHash);
            user.Projects = new List<ProjectModel>();
            user.AssignedTasks = new List<TaskItemModel>();

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "User registered successfully." });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Identifier || u.Email == request.Identifier);

            if (user == null || user.PasswordHash != HashPassword(request.Password))
                return Ok(new { success = false, message = "Invalid username/email or password." });

            return Ok(new { success = true, message = "Success" });
        }

        [HttpGet("all")] // all users listed
        public IActionResult GetAllUsers()
        {
            var users = _context.Users
           .Select(u => new
           {
               u.Username,
               u.Email
           })
             .ToList();

            return Ok(users);
        }
    }

    public class LoginRequest
    {
        public string Identifier { get; set; }
        public string Password { get; set; }
    }
}
