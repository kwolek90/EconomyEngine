using Common.MVC;
using MarketEconomy;
using Microsoft.AspNetCore.Mvc;

namespace StockMarketAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BidsController : BaseController
    {
        [HttpPost]
        public ActionResult Post(string marketName, string bookName, string customerId, double price, double amount)
        {
            MarketEngine.Instance.AddBid(marketName, bookName, customerId, price, amount);
            Response.StatusCode = 201;
            return Json(new {result = "OK"});
        }
    }
}