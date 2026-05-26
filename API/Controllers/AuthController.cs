using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        [HttpPost("login")]
        public IActionResult Login(string usuario, string senha)
        {
            if(usuario != "admin" || senha != "123")
            {
                return Unauthorized(new
                {
                    mensagem = "Usuário inválido"
                });
            }
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var chave = Encoding.UTF8.GetBytes("MICROONDAS_SUPER_CHAVE_PROJETO_CAIO_123");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, usuario)
                }),
                Expires = DateTime.UtcNow.AddHours(2),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(chave), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            string tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                token = tokenString
            });
        }
    }
}
