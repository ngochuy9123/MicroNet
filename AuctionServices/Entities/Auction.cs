namespace AuctionServices.Entities
{
	public class Auction
	{
		public Guid Id { get; set; }
		public int RevervePrice { get; set; } = 0;
		public string Seller {  get; set; }
		public string Winner { get; set; } = string.Empty;
		public int SoldAmount	{ get; set; }
		public int CurrentHighBid { get; set; }
		public DateTime AuctionEnd { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
		
		public Status Status { get; set; }
		public Item Item { get; set; }
		
	}
}
