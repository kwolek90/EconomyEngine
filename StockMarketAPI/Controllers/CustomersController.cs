using System;
using MarketEconomy;
using Microsoft.AspNetCore.Mvc;

namespace StockMarketAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        [HttpGet]
        public JsonResult Get(string marketName, string id)
        {
            var customer = MarketEngine.Instance.GetMarketCustomerById(marketName,id);
            return Json(customer);
        }

        [HttpPost]
        public JsonResult Post(string marketName, string name, double money)
        {
            var customer = MarketEngine.Instance.AddMarketCustomer(marketName, name, money);
            Response.StatusCode = 201;
            return Json(customer);
        }
    }
}