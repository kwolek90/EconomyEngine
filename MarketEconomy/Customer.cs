using System.Collections.Generic;
using Common;

namespace MarketEconomy
{
    public class Customer
    {
        public Customer()
        {
            Goods = new Dictionary<string, Good>();
            BlockedGoods = new Dictionary<string, Good>();
        }
        public string Name { get; set; }
        public string Id { get; set; }
        public double Money { get; set; }
        public double BlockedMoney { get; set; }
        
        public Dictionary<string,Good> Goods { get; set; }
        public Dictionary<string,Good> BlockedGoods { get; set; }


        public OperationResponse<Offer> PrepareAsk(string goodName, double price, double amount)
        {
            var response = new OperationResponse<Offer>();
            
            if (!Goods.ContainsKey(goodName))
            {
                Goods[goodName] = new Good();
            }

            if (Money < price * amount)
            {
                response.AddError("money","Not enough money.");
                return response;
            }

            Money -= price * amount;
            BlockedMoney += price * amount;
            
            var offer = new Offer(this, price, amount);
            response.Response = offer;
            
            return response;
        }
        
        public OperationResponse<Offer> PrepareBid(string goodName, double price, double amount)
        {
            var response = new OperationResponse<Offer>();
            if (!Goods.ContainsKey(goodName) && Goods.GetValueOrDefault(goodName).Amount < amount)
            {
                response.AddError("amount", "Not enough goods.");
                return response;
            }
            
            if (!BlockedGoods.ContainsKey(goodName))
            {
                BlockedGoods[goodName] = new Good();
            }

            Goods.GetValueOrDefault(goodName).Amount -= amount;
            BlockedGoods.GetValueOrDefault(goodName).Amount += amount;
            var offer = new Offer(this, price, amount);
            response.Response = offer;
            
            return response;
        }
    }
}