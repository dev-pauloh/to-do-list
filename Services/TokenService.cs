using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Todo.Models;

namespace Todo.Services
{
    public class TokenService
    {
        //Serviço para gerar o token
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.JwtKey); //Tranforma em um array de caracteres
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, user.Email)
                }
                    
                    ),
                Expires = DateTime.UtcNow.AddHours(8), //Tempo de expiração do token
                SigningCredentials = new SigningCredentials( //Como o token será gerado e lido posteriormente
                    new SymmetricSecurityKey(key),// A mesma chave usada para encriptar, será usada para desencriptar (pede um array de bytes)
                    SecurityAlgorithms.HmacSha256Signature) //Algoritmo para encriptar e desencriptar
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
