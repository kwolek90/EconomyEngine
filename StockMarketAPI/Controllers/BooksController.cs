using System.Linq.Expressions;
using MarketEconomy;
using Microsoft.AspNetCore.Mvc;

namespace StockMarketAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : Controller
    {
        [HttpGet]
        public JsonResult Get(string marketName, string name)
        {
            if (string.IsNullOrWhiteSpace(marketName) || string.IsNullOrWhiteSpace(name))
            {
                return Json(new {result = "Error"});
            }

            return Json(MarketEngine.Instance.GetMarketBookByName(marketName, name));
        }

        [HttpPost]
        public JsonResult Post(string marketName, string name)
        {
            if (string.IsNullOrWhiteSpace(marketName) || string.IsNullOrWhiteSpace(name))
            {
                return Json(new {result = "Error"});
            }

            MarketEngine.Instance.CreateMarketBook(marketName, name);
            Response.StatusCode = 201;
            return Json(new {result = "OK"});

        }
    }
}