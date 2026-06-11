using DataAccess.Models;
using FluentValidation;

namespace BusinessLogic.Validation
{
	public class ProductValidator: AbstractValidator<Product>
	{
		public ProductValidator()
		{
			RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
			RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Цена не может быть меньше 0");
			RuleFor(x => x.Count).GreaterThanOrEqualTo(0).WithMessage("Количество товаров не может быть меньше 0"); 
		}
	}
}
