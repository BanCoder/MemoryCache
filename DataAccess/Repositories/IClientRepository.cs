using DataAccess.Models;

namespace DataAccess.Repositories
{
	public interface IClientRepository
	{
		Task CreateAsync(Client client, CancellationToken cancellationToken = default);
		Task<List<Client>> GetAllAsync(CancellationToken cancellationToken = default);
		Task<Client?> GetByNameAsync(string clientName, CancellationToken cancellationToken = default);
		Task<Product?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default);
		Task DeleteAsync(Client client, CancellationToken cancellationToken = default); 
	}
}
