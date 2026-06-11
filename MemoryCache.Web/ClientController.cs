using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemoryCache.Web
{
	[ApiController]
	[Route("clients")]
	public class ClientController: ControllerBase
	{
		private readonly IClientService _clientService;
		public ClientController(IClientService clientService)
		{
			_clientService = clientService;
		}
		[HttpPost]
		public async Task<IActionResult> CreateAsync(string name, [FromBody] List<int> productId)
		{
			await _clientService.Create(name, productId); 
			return Ok(name);
		}
		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			var clients = await _clientService.GetAll();
			return Ok(clients); 
		}
		[HttpDelete("{name}")]
		public async Task<IActionResult> DeleteAsync([FromRoute] string name)
		{
			await _clientService.Delete(name);
			return Ok();
		}
	}
}
