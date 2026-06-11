using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using FluentValidation; 
namespace DataAccess.Repositories
{
	public class ClientRepository : IClientRepository
	{
		private readonly ProductDbContext _context;
		private readonly IValidator<Client> _clientValidator;
		public ClientRepository(ProductDbContext context, IValidator<Client> clientValidator)
		{
			_context = context;
			_clientValidator = clientValidator;
		}
		public async Task CreateAsync(Client client, CancellationToken cancellationToken)
		{
			_clientValidator.ValidateAndThrow(client);
			await _context.Clients.AddAsync(client, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);
		}
		public async Task<List<Client>> GetAllAsync(CancellationToken cancellationToken)
		{
			return await _context.Clients.ToListAsync(cancellationToken); 
		}
		public async Task<Client?> GetByNameAsync(string clientName, CancellationToken cancellationToken)
		{
			return await _context.Clients.FirstOrDefaultAsync(x => x.Name == clientName, cancellationToken); 
		}
		public async Task<Product?> GetProductByIdAsync(int productId, CancellationToken cancellationToken)
		{
			return await _context.Products.FirstOrDefaultAsync(x => x.Id == productId, cancellationToken); 
		}
		public async Task DeleteAsync(Client client, CancellationToken cancellationToken)
		{
			 _context.Clients.Remove(client);
			await _context.SaveChangesAsync(cancellationToken); 
		}
	}
}
