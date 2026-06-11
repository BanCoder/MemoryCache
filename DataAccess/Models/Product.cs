namespace DataAccess.Models
{
	public class Product
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public double Price { get; set; }
		public int Count { get; set; }
		public int? ClientId { get; set; }
		public Client? Client { get; set; }
	}
}
