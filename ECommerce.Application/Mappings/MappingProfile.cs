using AutoMapper;
using ECommerce.Domain.Entities;

// ↓↓↓ ТУТ БУЛА ПОМИЛКА: Products мають префікс ECommerce ↓↓↓
using ECommerce.Application.Features.Products.Queries.GetProductById;

// ↓↓↓ А ці, судячи з минулих помилок, префіксу НЕ мають ↓↓↓
using Application.Features.Users.Queries.GetUserById;
using Application.Features.Orders.Queries.GetOrderById;
using Application.Features.Orders.Queries.GetOrdersByUserId;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Продукти
            CreateMap<Product, ProductDto>();

            // Користувачі
            CreateMap<User, UserDto>();

            // Замовлення
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Order, OrderListDto>()
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                 .ForMember(dest => dest.ItemsCount, opt => opt.MapFrom(src => src.OrderItems.Count));

            // Елементи замовлення
            // Тут використовуємо повний шлях, бо OrderItemDto є і в CreateOrder, і в GetById
            CreateMap<OrderItem, Application.Features.Orders.Queries.GetOrderById.OrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
        }
    }
}