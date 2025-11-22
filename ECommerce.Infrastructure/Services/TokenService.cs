using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Guid userId, string email, string firstName, string lastName)
        {
            // 1. Створюємо список "тверджень" (Claims) про користувача
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()), // ID користувача
                new Claim(JwtRegisteredClaimNames.Email, email),           // Email
                new Claim("firstName", firstName),                         // Додаткові дані
                new Claim("lastName", lastName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Унікальний ID токена
            };

            // 2. Отримуємо ключ з налаштувань
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 3. Створюємо токен
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2), // Токен діє 2 години
                signingCredentials: creds
            );

            // 4. Повертаємо токен як рядок
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}