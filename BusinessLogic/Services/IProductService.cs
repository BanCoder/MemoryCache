using DataAccess.Models;

namespace BusinessLogic.Services
{
	public interface IProductService
	{
		Task CreateAsync(string name, double price, int count);
		Task<List<Product?>> GetAllAsync();
		Task<Product?> GetByIdAsync(int id);
		Task UpdateAsync(int id, string name, double price, int count);
		Task DeleteAsync(int id); 
	}
}
