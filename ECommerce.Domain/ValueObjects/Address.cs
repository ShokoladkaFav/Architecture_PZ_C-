using ECommerce.Domain.Common;
using System.Collections.Generic; // Для IEnumerable

namespace ECommerce.Domain.ValueObjects
{
    public class Address : ValueObject
    {
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string PostalCode { get; private set; }
        public string Country { get; private set; }

        // Приватний конструктор для Entity Framework Core
        private Address() { }

        public Address(string street, string city, string state, string postalCode, string country)
        {
            // Перевірки інваріантів
            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Street cannot be empty.", nameof(street));
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City cannot be empty.", nameof(city));
            // Додай інші перевірки, якщо потрібно

            Street = street;
            City = city;
            State = state;
            PostalCode = postalCode;
            Country = country;
        }

        // Реалізація абстрактного методу для порівняння ValueObject
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Street;
            yield return City;
            yield return State;
            yield return PostalCode;
            yield return Country;
        }

        // Можна додати корисні методи, наприклад, для форматування адреси
        public string ToFullString()
        {
            return $"{Street}, {City}, {State} {PostalCode}, {Country}";
        }
    }
}