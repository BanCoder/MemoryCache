using DataAccess.Models;
using FluentValidation;

namespace BusinessLogic.Validation
{
	public class ClientValidator: AbstractValidator<Client>
	{
		public ClientValidator()
		{
			RuleFor(x => x.Name).MaximumLength(20).NotEmpty().WithMessage("Имя не может быть пустым");
			RuleForEach(x => x.Products).SetValidator(new ClientOrderValidator()); 
		}
	}
	public class ClientOrderValidator : AbstractValidator<Product>
	{
		public ClientOrderValidator()
		{
			RuleFor(x => x.Name).NotEmpty().MaximumLength(50); 
		}
	}
}
