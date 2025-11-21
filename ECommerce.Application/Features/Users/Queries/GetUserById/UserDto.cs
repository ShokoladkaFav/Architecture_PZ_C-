using System;

namespace Application.Features.Users.Queries.GetUserById
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        // Адресу поки можна опустити, або додати як простий об'єкт, якщо потрібно
    }
}