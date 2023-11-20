using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Agendo.Shared;
using Agendo.AuthAPI.Model;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Agendo.AuthAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace Agendo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public AuthController(IConfiguration configuration, IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult<User> Login(UserLoginDto request)
        {            var x = _authService.Login(request.Username).Result;

            if(x.Username.IsNullOrEmpty())
            {
                return BadRequest("Not found");
            }

            if(!BCrypt.Net.BCrypt.Verify(request.Password, x.PasswordHash))
            {
                return BadRequest("Not found");
            }
            //should be replaced with real token creation 

            //token genrator and read db users password
            string token = CreateToken(x);


            return Ok(token);
        }

        [HttpPost("register")]
        public ActionResult<User> Register(UserLoginDto request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);


            _authService.Register(new User
            {
                Username = request.Username,
                PasswordHash = passwordHash

            });
            return Ok(User);

        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Actor, user.EmpID)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                          _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds

                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}