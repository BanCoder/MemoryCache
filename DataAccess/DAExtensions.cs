using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
	public static class DAExtensions
	{
		public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("DefaultConnection") ?? configuration["ConnectionSettings:sqlConnection"];
			serviceCollection.AddScoped<IProductRepository, ProductRepository>();
			serviceCollection.AddScoped<IClientRepository, ClientRepository>();
			serviceCollection.AddDbContext<ProductDbContext>(x =>
			{
				x.UseNpgsql(connectionString); 
			}); 
			return serviceCollection;
		}
	}
}
