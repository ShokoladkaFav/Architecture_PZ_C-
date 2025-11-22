using System;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Guid userId, string email, string firstName, string lastName);
    }
}