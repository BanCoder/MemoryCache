using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BusinessLogic.Services
{
	public class RedisCacheService: ICacheService
	{
		private readonly IDistributedCache _cache; 
		private readonly ILogger<RedisCacheService> _logger;
		public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
		{
			_cache = cache;
			_logger = logger;
		}
		public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
		{
			var data = await _cache.GetStringAsync(key, cancellationToken);
			return data == null ? default : JsonSerializer.Deserialize<T>(data); 
		}
		public async Task SetAsync<T>(string key, T value, TimeSpan? slidingExperation = null, TimeSpan? absoluteExpiration = null)
		{
			var options = new DistributedCacheEntryOptions();
			if (slidingExperation.HasValue)
			{
				options.SetSlidingExpiration(slidingExperation.Value);
			}
			if (absoluteExpiration.HasValue)
			{
				options.SetAbsoluteExpiration(absoluteExpiration.Value);
			}
			var jsonData = JsonSerializer.Serialize(value);
			await _cache.SetStringAsync(key, jsonData, options); 
		}
		public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
		{
			await _cache.RemoveAsync(key, cancellationToken); 
		}
	}
}
