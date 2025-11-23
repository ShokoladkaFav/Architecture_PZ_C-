using ECommerce.Application.Features.Products.Queries.GetProductById;  // Для ProductDto
using AutoMapper; // Для мапінгу
using ECommerce.Application.Features.Products.Queries.GetProductById;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Memory; // <--- Для кешування
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache; // <--- Сервіс кешування

        // Інжектимо всі залежності
        public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            // Ключ для кешу (унікальна назва для цього списку)
            const string cacheKey = "productsList";

            // 1. ПЕРЕВІРКА КЕШУ
            // Якщо дані вже є в пам'яті, беремо їх звідти і не чіпаємо БД
            if (_cache.TryGetValue(cacheKey, out List<ProductDto> cachedProducts))
            {
                return cachedProducts;
            }

            // 2. ЯКЩО В КЕШІ НЕМАЄ - ЙДЕМО В БАЗУ
            var repository = _unitOfWork.Repository<Product>();
            var products = await repository.GetAllAsync();

            // Використовуємо AutoMapper замість ручного перебору
            var productsDto = _mapper.Map<List<ProductDto>>(products);

            // 3. ЗБЕРІГАЄМО В КЕШ
            // Налаштовуємо час життя кешу
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5)) // Видалити через 5 хвилин
                .SetSlidingExpiration(TimeSpan.FromMinutes(2)); // Видалити, якщо ніхто не запитував 2 хвилини

            _cache.Set(cacheKey, productsDto, cacheOptions);

            return productsDto;
        }
    }
}