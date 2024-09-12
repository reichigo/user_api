using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mypethere.User.Domain.Entities.Credencial
{
    public record Token(Guid UserId)
    {
        // Configurações do Kong para o JWT
        private static readonly string key = "npKieAWkZCTJP5SoN4L86sPrS3BP6IG9"; // key do Kong
        private static readonly string secret = "X8Kv75QQYpgL5KOPAGvtYSjvK5YFUPth"; // secret do Kong
        private static readonly string algorithm = SecurityAlgorithms.HmacSha256; // Certifique-se de usar o algoritmo correto
        private static readonly string sub = "36edea14-d2ed-484f-8587-ebf59aee6b39"; // ID do consumidor no Kong

        public string GetToken()
        {
            // Define as claims do JWT
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, UserId.ToString()), // Subclaim como o ID do consumidor
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // ID único do JWT
                new Claim(JwtRegisteredClaimNames.Iss, key) // Claim 'iss' é o mesmo que a chave no Kong
            };

            // Configura a chave de segurança e o algoritmo de assinatura
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, algorithm);

            // Cria o token JWT
            var token = new JwtSecurityToken(
                issuer: key, // Issuer deve ser a chave do Kong
                audience: null, // Kong geralmente não verifica o audience por padrão, então pode ser null
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1), // Define a expiração do token
                signingCredentials: credentials
            );

            // Gera o token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            string jwt = tokenHandler.WriteToken(token);

            return jwt;
        }
    }
}
