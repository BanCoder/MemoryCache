using DataAccess.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
	public class ProductRepository: IProductRepository
	{
		private readonly ProductDbContext _context;
		private readonly IValidator<Product> _productValidator;
		public ProductRepository(ProductDbContext context, IValidator<Product> productValidator)
		{
			_context = context;
			_productValidator = productValidator;
		}
		public async Task CreateAsync(Product product, CancellationToken cancellationToken = default)
		{
			_productValidator.ValidateAndThrow(product);
			await _context.Products.AddAsync(product, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);
		}
		public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			return await _context.Products.ToListAsync(cancellationToken); 
		}
		public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
		{
			return await _context.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
		}
		public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
		{
			_context.Products.Update(product);
			await _context.SaveChangesAsync(cancellationToken);
		}
		public async Task DeleteAsync(Product product, CancellationToken cancellationToken = default)
		{
			_context.Products.Remove(product);
			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
