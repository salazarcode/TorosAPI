using Infra.DTOs.Requests;
using Infra.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
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

        [HttpGet]
        public IActionResult Hello()
        {
            return Ok("Despleagdo adecuadamente con GithubActions.");
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

            var rsa = new RSACryptoServiceProvider();
            rsa.ImportFromPem(System.IO.File.ReadAllText("private.key"));
            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

            //var encodedKey = Encoding.UTF8.GetBytes(key);
            //var credentials = new SigningCredentials(new SymmetricSecurityKey(encodedKey), SecurityAlgorithms.HmacSha256Signature);
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                SigningCredentials = signingCredentials,
                Audience = audience,
                Issuer = issuer,
                IssuedAt = DateTime.UtcNow,
                Subject = new ClaimsIdentity(new[] {
                    new Claim("sub", username)
                }),
                Expires = expiration
            });

            return tokenHandler.WriteToken(token);
        }
    }
}
