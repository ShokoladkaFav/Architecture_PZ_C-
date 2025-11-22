using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace ECommerce.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                // Отримуємо дані про користувача з поточного HTTP-запиту
                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null)
                {
                    return Guid.Empty;
                }

                // Шукаємо Claim, який відповідає за ID (NameIdentifier)
                // Ми його туди записали в TokenService: new Claim(JwtRegisteredClaimNames.Sub, userId.ToString())
                // JwtRegisteredClaimNames.Sub часто мапиться на ClaimTypes.NameIdentifier
                var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)
                              ?? user.FindFirst("sub"); // Про всяк випадок шукаємо і так

                if (idClaim != null && Guid.TryParse(idClaim.Value, out var userId))
                {
                    return userId;
                }

                return Guid.Empty;
            }
        }
    }
}