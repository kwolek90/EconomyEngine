using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Common.MVC;
using MarketEconomy;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public ActionResult Patch(string marketName, string id, string operation, double? money, JsonElement goods)
        {

            return Json(goods);
            var deserializedGoods = new List<Good>();
            foreach (var jsonGood in goods.EnumerateArray())
            {
                var good =new Good();
                good.Name = jsonGood.GetProperty("name").GetString();
                good.Amount = jsonGood.GetProperty("amount").GetDouble();
            }

            return Json(deserializedGoods);
            // var response = MarketEngine.Instance.UpdateMarketCustomer(marketName, id,operation, money,deserializedGoods);
            // return PrepareResponse(response);
        }
    }
}