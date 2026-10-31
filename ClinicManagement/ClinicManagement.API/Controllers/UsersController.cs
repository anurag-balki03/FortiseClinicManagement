
using ClinicManagement.Api.Data;
using ClinicManagement.Api.DTOs;
using ClinicManagement.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly ClinicDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UsersController(
            ClinicDbContext context,
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
        {
            var users = await _context.Users
                .AsNoTracking()
                .Select(u => new UserResponse
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Username = u.Username,
                    Role = u.Role,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/users/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserResponse>> GetUserById(int id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.UserId == id)
                .Select(u => new UserResponse
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Username = u.Username,
                    Role = u.Role,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserResponse>> CreateUser(
            [FromBody] CreateUserRequest request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    message = "Request body is required."
                });
            }

            if (string.IsNullOrWhiteSpace(request.FullName) ||
                string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new
                {
                    message = "Full name, username and password are required."
                });
            }

            var fullName = request.FullName.Trim();
            var username = request.Username.Trim();

            if (fullName.Length > 50)
            {
                return BadRequest(new
                {
                    message = "Full name cannot exceed 50 characters."
                });
            }

            if (username.Length > 50)
            {
                return BadRequest(new
                {
                    message = "Username cannot exceed 50 characters."
                });
            }

            if (request.Password.Length < 8)
            {
                return BadRequest(new
                {
                    message = "Password must contain at least 8 characters."
                });
            }

            if (request.Role < 1 || request.Role > 3)
            {
                return BadRequest(new
                {
                    message = "Invalid role. Use 1=Admin, 2=Doctor, 3=Receptionist."
                });
            }

            var usernameExists = await _context.Users
                .AnyAsync(u => u.Username == username);

            if (usernameExists)
            {
                return Conflict(new
                {
                    message = "Username already exists."
                });
            }

            var user = new User
            {
                FullName = fullName,
                Username = username,
                Role = request.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user,
                request.Password
            );

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            var response = new UserResponse
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Username = user.Username,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = user.UserId },
                response
            );
        }
    }
}
