using BusinessLogic.Services;
using BusinessLogic.Validation;
using DataAccess.Models;
using DataAccess.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection; 
namespace BusinessLogic
{
	public static class BLExtensions
	{
		public static IServiceCollection AddBusinessLogic(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<IProductService, ProductService>();
			serviceCollection.AddScoped<IClientService, ClientService>();
			serviceCollection.AddScoped<IValidator<Product>, ProductValidator>();
			serviceCollection.AddScoped<IValidator<Client>, ClientValidator>();
			//serviceCollection.AddSingleton<ICacheService, RedisCacheService>();
			//serviceCollection.AddScoped<IProductService, ProductServiceWithRedis>();
			return serviceCollection; 
		}
	}
}
