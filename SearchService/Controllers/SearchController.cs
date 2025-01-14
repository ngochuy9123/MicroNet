using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SearchController : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams)
		{
			var query = DB.PagedSearch<Item, Item>();

			// Tính toán các giá trị thời gian trước
			var now = DateTime.UtcNow;
			var sixHoursLater = now.AddHours(6);

			if (!string.IsNullOrEmpty(searchParams.SearchTerm))
			{
				query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
			}

			// Sắp xếp theo các tiêu chí
			query = searchParams.OrderBy switch
			{
				"make" => query.Sort(x => x.Ascending(a => a.Make)),
				"new" => query.Sort(x => x.Ascending(a => a.CreatedAt)),
				_ => query.Sort(x => x.Ascending(a => a.AuctionEnd)),
			};

			// Lọc theo các tiêu chí
			query = searchParams.FilterBy switch
			{
				"finished" => query.Match(a => a.AuctionEnd < now),
				"endingSoon" => query.Match(a => a.AuctionEnd > now && a.AuctionEnd <= sixHoursLater),
				_ => query.Match(a => a.AuctionEnd > now),
			};

			// Lọc theo seller
			if (!string.IsNullOrEmpty(searchParams.Seller))
			{
				query.Match(x => x.Seller == searchParams.Seller);
			}

			// Lọc theo winner
			if (!string.IsNullOrEmpty(searchParams.Winner))
			{
				query.Match(x => x.Winner == searchParams.Winner);
			}

			// Phân trang
			query.PageNumber(searchParams.PageNumber);
			query.PageSize(searchParams.PageSize);

			// Thực thi truy vấn
			var result = await query.ExecuteAsync();

			return Ok(new
			{
				results = result.Results,
				pageCount = result.PageCount,
				totalCount = result.TotalCount
			});
		}
	}
}
