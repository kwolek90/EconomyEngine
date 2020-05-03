using MarketEconomy;
using Microsoft.AspNetCore.Mvc;

namespace StockMarketAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BidsController : Controller
    {
        [HttpPost]
        public JsonResult Post(string marketName, string bookName, string customerId, double price, double amount)
        {
            MarketEngine.Instance.AddBid(marketName, bookName, customerId, price, amount);
            Response.StatusCode = 201;
            return Json(new {result = "OK"});
        }
    }
}