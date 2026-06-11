using DataAccess.Models;

namespace BusinessLogic.Services
{
	public interface IClientService
	{
		Task Create(string clientName, List<int> productId); 
		Task<List<Client>> GetAll();
		Task Delete(string clientName);
	}
}
