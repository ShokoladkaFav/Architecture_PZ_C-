using FluentValidation;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct
{
    // Цей клас описує правила для CreateProductCommand
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            // Правило: Назва не може бути порожньою і не довша за 100 символів
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

            // Правило: Опис не порожній
            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            // Правило: Ціна має бути більшою за 0
            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

            // Правило: Кількість на складі не може бути від'ємною
            RuleFor(p => p.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} cannot be negative.");

            // Правило: Категорія обов'язкова
            RuleFor(p => p.Category)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}