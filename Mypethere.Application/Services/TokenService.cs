
using Microsoft.IdentityModel.Tokens;

using Mypethere.User.Domain.Repositories;

using MyPethere.User.CrossCutting.Exceptions;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Mypethere.User.Application.Services;

public class TokenService(ITokenRepository tokenRepository) : ITokenService
{
    // Configurações do Kong para o JWT
    private static readonly string key = "npKieAWkZCTJP5SoN4L86sPrS3BP6IG9"; // key do Kong
    private static readonly string secret = "X8Kv75QQYpgL5KOPAGvtYSjvK5YFUPth"; // secret do Kong
    private static readonly string algorithm = SecurityAlgorithms.HmacSha256; // Certifique-se de usar o algoritmo correto

    public (string AccessToken, string RefreshToken) CreateTokens(Guid userId)
    {
        var accessToken = GenerateAccessToken(userId);
        var refreshToken = GenerateRefreshToken();

        return (accessToken, refreshToken);
    }

    private string GenerateAccessToken(Guid userId)
    {
        // Define as claims do JWT
        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()), // Subclaim como o ID do consumidor
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
            expires: DateTime.UtcNow.AddMinutes(15), // Define a expiração do token (15 minutos neste exemplo)
            signingCredentials: credentials
        );

        // Gera o token JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        string jwt = tokenHandler.WriteToken(token);

        return jwt;
    }

    private string GenerateRefreshToken()
    {
        // Gera um refresh token aleatório seguro
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private bool ValidateRefreshToken(string refreshToken, string refreshToken1)
    {
        return refreshToken.Equals(refreshToken1);
    }

    public async Task<(string AccessToken, string RefreshToken)> RefreshTokens(Guid userId, string refreshToken)
    {
        var dbRefreshToken = await tokenRepository.GetRefreshTokenById(userId);

        if (!ValidateRefreshToken(dbRefreshToken, refreshToken))
        {
            throw new BadRequestException("Invalid refresh token", System.Net.HttpStatusCode.BadRequest);
        }

        // Gere novos tokens
        var newAccessToken = GenerateAccessToken(userId);
        var newRefreshToken = GenerateRefreshToken();

        await RevokeRefreshToken(userId);

        await tokenRepository.InsertRefreshToken(userId, newRefreshToken);

        return (newAccessToken, newRefreshToken);
    }

    public Task RevokeRefreshToken(Guid userId)
    {
        return tokenRepository.RevokeRefreshToken(userId);
    }
}