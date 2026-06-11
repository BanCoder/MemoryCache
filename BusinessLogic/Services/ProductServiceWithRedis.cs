using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
	public class ProductServiceWithRedis: IProductService
	{
		private readonly IProductRepository _repository; 
		private readonly ICacheService _cacheService;
		private readonly ILogger<ProductServiceWithRedis> _logger;
		public ProductServiceWithRedis(IProductRepository repository, ICacheService cacheService, ILogger<ProductServiceWithRedis> logger)
		{
			_repository = repository;
			_cacheService = cacheService;
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
			await _cacheService.RemoveAsync("all_products");
			_logger.LogInformation("Кэш инвалидирован: all_products");
		}

		public async Task<List<Product?>> GetAllAsync()
		{
			const string cacheKey = "all_products";
			var products = await _cacheService.GetAsync<List<Product?>>(cacheKey);
			if (products != null)
			{
				return products;
			}
			products = await _repository.GetAllAsync();
			await _cacheService.SetAsync(cacheKey, products, slidingExperation: TimeSpan.FromMinutes(2), absoluteExpiration: TimeSpan.FromMinutes(10));
			return products;
		}
		public async Task<Product?> GetByIdAsync(int id)
		{
			var cacheKey = $"product_{id}"; 
			var product = await _cacheService.GetAsync<Product>(cacheKey);
			if(product != null)
			{
				return product;
			}
			product = await _repository.GetByIdAsync(id);
			if (product != null)
			{
				await _cacheService.SetAsync(cacheKey, product, slidingExperation: TimeSpan.FromMinutes(5), absoluteExpiration: TimeSpan.FromHours(1));
			}
			return product;
		}
		public async Task UpdateAsync(int id, string name, double price, int count)
		{
			var product = await _repository.GetByIdAsync(id);
			if (product == null)
			{
				_logger.LogWarning("Product {Id} not found", id);
				return;
			}

			if (!string.IsNullOrEmpty(name))
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

			await _cacheService.RemoveAsync($"product_{id}");
			await _cacheService.RemoveAsync("all_products");
			_logger.LogInformation("Кэш инвалидирован для product {Id}", id);
		}

		public async Task DeleteAsync(int id)
		{
			var product = await _repository.GetByIdAsync(id);
			if (product == null)
			{
				_logger.LogWarning("Product {Id} not found", id);
				return;
			}

			await _repository.DeleteAsync(product);

			await _cacheService.RemoveAsync($"product_{id}");
			await _cacheService.RemoveAsync("all_products");
			_logger.LogInformation("Кэш инвалидирован для product {Id}", id);
		}
	}
}
