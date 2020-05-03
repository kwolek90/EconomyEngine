using System.Linq.Expressions;
using Common.MVC;
using MarketEconomy;
using Microsoft.AspNetCore.Mvc;

namespace StockMarketAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : BaseController
    {
        [HttpGet]
        public ActionResult Get(string marketName, string name)
        {
            if (string.IsNullOrWhiteSpace(marketName) || string.IsNullOrWhiteSpace(name))
            {
                return Json(new {result = "Error"});
            }

            return Json(MarketEngine.Instance.GetMarketBookByName(marketName, name));
        }

        [HttpPost]
        public ActionResult Post(string marketName, string name)
        {
            var response = MarketEngine.Instance.CreateMarketBook(marketName, name);
            return PrepareResponse(response);

        }
    }
}