using Common.MVC;
using Microsoft.AspNetCore.Mvc;
using MarketEconomy;

namespace StockMarketAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarketsController : BaseController
    {
        [HttpGet]
        public ActionResult Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Json(MarketEngine.Instance.GetAllMarketsNames());   
            }

            return Json(MarketEngine.Instance.GetMarketByName(name));
        }

        [HttpPost]
        public ActionResult Post(string name)
        {
            var response = MarketEngine.Instance.CreateMarket(name);
            return PrepareResponse(response);
        }
    }
}