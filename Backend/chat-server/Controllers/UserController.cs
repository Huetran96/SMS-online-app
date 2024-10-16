using chat_server.data;
using chat_server.DTOs;
using chat_server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace chat_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;

        public UserController(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost]
        [Route("create-admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] RegisterDto request)
        {
            var admin = new IdentityRole(Role.ADMIN);
            var user = new IdentityRole(Role.USER);
            
            if( !await _roleManager.RoleExistsAsync(Role.ADMIN))
            {
                await _roleManager.CreateAsync(admin);
            }
            if ( !await _roleManager.RoleExistsAsync(Role.USER))
            {
                await _roleManager.CreateAsync(user);
            }
            var admins = await _userManager.GetUsersInRoleAsync(Role.ADMIN);
            if (admins.Count > 0)
            {
                return BadRequest("Admin is existed.");
            }
            var newAdmin = new User()
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(newAdmin, request.Password);
            if ( ! result.Succeeded)
            {
                return BadRequest("Something error");
            }
            await _userManager.AddToRoleAsync(newAdmin, Role.ADMIN);
            var profile = new Profile() { UserId = newAdmin.Id };
            await _context.Profiles.AddAsync(profile);
            await _context.SaveChangesAsync();
            return Ok("Create Admin successed");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {


            var isEmail = await _userManager.FindByEmailAsync(request.Email);
            var isPhoneNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
            if (isEmail != null)
            {
                return BadRequest("Email is existed");
            }
            if (isPhoneNumber != null)
            {
                return BadRequest("Phone number is existed");
            }
            var newUser = new User()
            {
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email

            };
            var result = await _userManager.CreateAsync(newUser, request.Password);


            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.ToList());
            }
            await _userManager.AddToRoleAsync(newUser, Role.USER);

            var profile = new Profile() { UserId = newUser.Id };
            await _context.Profiles.AddAsync(profile);
            await _context.SaveChangesAsync();
            return Ok("Register successed");

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest("User not exist");
            }
            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
            {
                return BadRequest("Password's not correct. Try again!");
            }
            var token = await GenerateToken(user);
            

            return Ok(token); 
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logout");
        }

        [Authorize(Roles = Role.ADMIN)]
        [HttpGet]
        [Route("all-users")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        [HttpGet]
        [Route("my-profile")]
        public async Task<IActionResult> MyProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
            return Ok(profile);

        }

        private async Task<IActionResult> GenerateToken( User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim (ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTID", Guid.NewGuid().ToString())
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
            var tokenObject = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: claims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );
            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return Ok(token);

        }
    }
}
