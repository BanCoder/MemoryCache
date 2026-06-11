using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
namespace BusinessLogic.Services
{
	public class ClientService: IClientService
	{
		private readonly IClientRepository _clientRepository;
		private readonly IMemoryCache _memoryCache; 
		private readonly ILogger<ClientService> _logger;
		public ClientService(IClientRepository clientRepository, IMemoryCache memoryCache, ILogger<ClientService> logger)
		{
			_clientRepository = clientRepository;
			_memoryCache = memoryCache;
			_logger = logger;
		}

		public async Task Create(string clientName, List<int> productIds)
		{
			var client = new Client
			{
				Name = clientName,
				Products = new List<Product>()
			}; 
			foreach(var productId in productIds)
			{
				var product = await _clientRepository.GetProductByIdAsync(productId);
				if (product != null)
				{
					client.Products.Add(product);
				}
			}
			await _clientRepository.CreateAsync(client);
			_memoryCache.Remove("all_clients");
			_logger.LogInformation("\"Кэш инвалидирован: all_clients\""); 
		}
		public async Task<List<Client>> GetAll()
		{
			var cacheKey = "all_clients";
			return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
			{
				_logger.LogInformation("Все клиенты загружены из бд");
				entry.SetSlidingExpiration(TimeSpan.FromMinutes(2)).SetAbsoluteExpiration(TimeSpan.FromMinutes(2)).SetPriority(CacheItemPriority.Normal); 
				return await _clientRepository.GetAllAsync();
			}); 
		}
		public async Task Delete(string clientName)
		{
			var client = await _clientRepository.GetByNameAsync(clientName);
			if(client == null)
			{
				Console.WriteLine("Client is not found"); 
				return;
			}
			await _clientRepository.DeleteAsync(client);
			_memoryCache.Remove($"client: {clientName}");
			_memoryCache.Remove("all_clients");
			_logger.LogInformation("Кэш инвалидирован для client: {ClientName} и all_clients", clientName);
		}
	}
}
