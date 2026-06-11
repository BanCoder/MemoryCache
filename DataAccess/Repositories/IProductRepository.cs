using DataAccess.Models;
namespace DataAccess.Repositories
{
	public interface IProductRepository
	{
		Task CreateAsync(Product product, CancellationToken cancellationToken = default);
		Task<List<Product?>> GetAllAsync(CancellationToken cancellationToken = default);
		Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
		Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
		Task DeleteAsync(Product product, CancellationToken cancellationToken = default); 
		
	}
}
