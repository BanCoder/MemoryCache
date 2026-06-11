using DataAccess.Models;
using Serilog; 
using DataAccess.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
	public class ProductService: IProductService
	{
		private readonly IProductRepository _repository;
		private readonly IMemoryCache _memoryCache;
		private readonly ILogger<ProductService> _logger; 

		public ProductService(IProductRepository repository, IMemoryCache memoryCache, ILogger<ProductService> logger)
		{
			_repository = repository;
			_memoryCache = memoryCache;
			_logger = logger;
		}
		public async Task CreateAsync(string name, double price, int count)
		{
			var product = new Product
			{
				Name = name,
				Price = price, 
				Count = count
			}; 
			await _repository.CreateAsync(product);
			_memoryCache.Remove("all_products");
			_logger.LogInformation("Кэш инвалидирован: all_products");
		}
		public async Task<List<Product?>> GetAllAsync()
		{
			var cacheKey = "all_products";
			return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
			{
				_logger.LogInformation("Все продукты из бд загружены");
				entry.SetSlidingExpiration(TimeSpan.FromMinutes(2)).SetAbsoluteExpiration(TimeSpan.FromMinutes(10)).SetPriority(CacheItemPriority.Normal);
				return await _repository.GetAllAsync();
			}); 
		}
		public async Task<Product?> GetByIdAsync(int id)
		{
			var cacheKey = $"product_{id}";
			return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
			{
				_logger.LogInformation("Продукт с id:{id} загружен из базы данных", id);
				entry.SetSlidingExpiration(TimeSpan.FromMinutes(5)).SetAbsoluteExpiration(TimeSpan.FromHours(1)).SetPriority(CacheItemPriority.High);
				return await _repository.GetByIdAsync(id);
			}); 
		}
		public async Task UpdateAsync(int id, string name, double price, int count)
		{
			var product = await _repository.GetByIdAsync(id);
			if(product == null)
			{
				Console.WriteLine("Product is not found"); 
			}
			if(name != null)
			{
				product.Name = name;
			}
			if (price > 0)
			{
				product.Price = price;
			}
			if (count > 0)
			{
				product.Count = count;
			}
			await _repository.UpdateAsync(product);
			var cacheKey = $"product_{product.Id}";
			_memoryCache.Remove(cacheKey); 
			_memoryCache.Remove("all_products");
			_logger.LogInformation("Кэш инвалидирован: {CacheKey} и all_products", cacheKey); 
		}
		public async Task DeleteAsync(int id)
		{
			var product = await _repository.GetByIdAsync(id);
			if (product == null)
			{
				Console.WriteLine("Product is not found"); 
			}
			await _repository.DeleteAsync(product);
			_memoryCache.Remove($"product_{id}");
			_memoryCache.Remove("all_products");
			_logger.LogInformation("Кэш инвалидирован для product {Id} и all_products", id); 
		}
	}
}
