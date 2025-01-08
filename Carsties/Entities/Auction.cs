namespace Carsties.Entities
{
	public class Auction
	{
		public Guid Id { get; set; }
		public int ReservePrice { get; set; }
		public string Seller {  get; set; }
		public string Winner { get; set; }
		public int? SoldAmount { get; set; }
		public int? CurrentHignBid { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
		public Status Status { get; set; }
		public Item Item { get; set; }

	}
}
