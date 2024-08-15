using MicroAuth.Requests;
using MicroAuth.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MicroAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Aquí validarías al usuario, por simplicidad asumimos que es válido
            if (request.Username != "andres" || request.Password != "12345")
                return Unauthorized();

            var expirationTime = DateTime.UtcNow.AddHours(12);
            var token = GenerateJwtToken(request.Username, expirationTime);
            return Ok(new AuthResponse { Token = token, Expiration = expirationTime });
        }

        private string GenerateJwtToken(string username, DateTime expiration)
        {
            var key = _configuration["Jwt:Key"];
            var audience = _configuration["Jwt:Audience"];
            var issuer = _configuration["Jwt:Issuer"];

            var encodedKey = Encoding.UTF8.GetBytes(key);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(encodedKey), SecurityAlgorithms.HmacSha256Signature);
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Audience = "micro_services" ,
                Issuer = issuer,
                Subject = new ClaimsIdentity(new[] {
                    new Claim("sub", username),
                    //new Claim(JwtRegisteredClaimNames.Aud, audience)    
                }),
                Expires = expiration,
            });

            //var audienceClaim = ((JwtSecurityToken)token).Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Aud);
            //Console.WriteLine($"Audience: {audienceClaim?.Value}");

            return tokenHandler.WriteToken(token);
        }
    }
}
