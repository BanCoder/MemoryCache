using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemoryCache.Web
{
	[ApiController]
	[Route("products")]
	public class ProductController: ControllerBase
	{
		private readonly IProductService _service;

		public ProductController(IProductService service)
		{
			_service = service;
		}
		[HttpPost]
		public async Task<IActionResult> CreateAsync(string name, double price, int count)
		{
			await _service.CreateAsync(name, price, count);
			return NoContent(); 
		}
		[HttpGet]
		public async Task<IActionResult> GetAllProducts()
		{
			var result = await _service.GetAllAsync();
			return Ok(result); 
		}
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetProductsById([FromRoute] int id)
		{
			var result = await _service.GetByIdAsync(id);
			return Ok(result);

		}
		[HttpPost("{id:int}")]
		public async Task<IActionResult> UpdateAsync([FromRoute] int id, string name, double price, int count)
		{
			await _service.UpdateAsync(id, name, price, count);
			return Ok();
		}
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteAsync([FromRoute] int id)
		{
			await _service.DeleteAsync(id);
			return Ok(); 
		}
	}
}
