namespace BusinessLogic.Services
{
	public interface ICacheService
	{
		Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
		Task SetAsync<T>(string key, T value, TimeSpan? slidingExperation = null, TimeSpan? absoluteExpiration = null);
		Task RemoveAsync(string key, CancellationToken cancellationToken = default);
	}
}
