using Microsoft.AspNetCore.Mvc;
using MarketEconomy;

namespace StockMarketAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarketsController : Controller
    {
        [HttpGet]
        public JsonResult Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Json(MarketEngine.Instance.GetAllMarketsNames());   
            }

            return Json(MarketEngine.Instance.GetMarketByName(name));
        }

        [HttpPost]
        public JsonResult Post(string name)
        {
            MarketEngine.Instance.CreateStockMarket(name);
            Response.StatusCode = 201;
            return Json(new {result = "OK"});
        }
    }
}