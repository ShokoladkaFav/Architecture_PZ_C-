using FluentValidation;

namespace Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            // Змінено валідацію для Guid
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(p => p.Category)
                .NotEmpty().WithMessage("Category is required.");
        }
    }
}