﻿using Common.MVC;
using MarketEconomy;
using Microsoft.AspNetCore.Mvc;

namespace StockMarketAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AsksController : BaseController
    {
        [HttpPost]
        public ActionResult Post(string marketName, string bookName, string customerId, double price, double amount)
        {
            var response = MarketEngine.Instance.AddAsk(marketName, bookName, customerId, price, amount);
            return PrepareResponse(response);
        }
    }
}