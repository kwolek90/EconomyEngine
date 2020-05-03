using System;
using Common.MVC;
using MarketEconomy;
using Microsoft.AspNetCore.Mvc;

namespace StockMarketAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : BaseController
    {
        [HttpGet]
        public ActionResult Get(string marketName, string id)
        {
            var customer = MarketEngine.Instance.GetMarketCustomerById(marketName,id);
            return Json(customer);
        }

        [HttpPost]
        public ActionResult Post(string marketName, string name, double money)
        {
            var response = MarketEngine.Instance.AddMarketCustomer(marketName, name, money);
            return PrepareResponse(response);
        }
    }
}