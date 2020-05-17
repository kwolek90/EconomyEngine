using System;
using System.Collections.Generic;
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
            var response = MarketEngine.Instance.GetMarketCustomerById(marketName,id);
            return PrepareResponse(response);
        }

        [HttpPost]
        public ActionResult Post(string marketName, string name, double money)
        {
            var response = MarketEngine.Instance.AddMarketCustomer(marketName, name, money);
            return PrepareResponse(response);
        }

        [HttpPatch]
        public ActionResult Patch(string marketName, string id, string operation, double? money, List<Good> goods)
        {
            var response = MarketEngine.Instance.UpdateMarketCustomer(marketName, id,operation, money,goods);
            return PrepareResponse(response);
        }
    }
}