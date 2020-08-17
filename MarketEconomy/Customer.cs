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
                Goods[goodName] = new Good() {Name = goodName};
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
            if (!Goods.ContainsKey(goodName))
            {
                response.AddError("amount", $"Customer has not good {goodName}.");
            }
            else if (Goods.GetValueOrDefault(goodName).Amount < amount)
            {
                response.AddError("amount", "Not enough goods.");
            }
            else
            {
                if (!BlockedGoods.ContainsKey(goodName))
                {
                    BlockedGoods[goodName] = new Good();
                }
                Goods.GetValueOrDefault(goodName).Amount -= amount;
                BlockedGoods.GetValueOrDefault(goodName).Amount += amount;
                var offer = new Offer(this, price, amount);
                response.Response = offer;
            }
            
            
            return response;
        }

        public OperationResponse BringGood(Good good)
        {
            var response = new OperationResponse();
            if (!Goods.ContainsKey(good.Name))
            {
                Goods[good.Name] = new Good() {Name = good.Name};
            }
            if (good.Amount < 0)
            {
                response.AddError("good", $"Amount of good {good.Name} to bring could not be less than 0.");
            }
            else
            {
                Goods[good.Name].Amount += good.Amount;   
            }

            return response;
        }
        
        
        public OperationResponse RetrieveGood(Good good)
        {
            var response = new OperationResponse();
            if (!Goods.ContainsKey(good.Name))
            {
                response.AddError("good", $"No good {good.Name} to retrieve.");
            }
            else
            {
                if (good.Amount < 0)
                {
                    response.AddError("good", $"Amount of good {good.Name} to retrieve could not be less than 0.");
                }
                else if(good.Amount > Goods[good.Name].Amount)
                {
                    response.AddError("good", $"Not enought good {good.Name}.");   
                }
                else
                {
                    Goods[good.Name].Amount -= good.Amount;   
                }
            }

            return response;
        }

        public OperationResponse<Customer> ModifyBelongings(string operation, double? money, List<Good> goods)
        {
            var response = new OperationResponse<Customer>();
            if (money.GetValueOrDefault() < 0)
            {
                response.AddError("money","Negative value of money.");
                money = 0;
            }
            if (operation == "add")
            {
                Money += money.GetValueOrDefault();
                foreach (var good in goods)
                {
                    response.Merge(BringGood(good));
                }
            }
            else if (operation == "retrieve")
            {
                if (Money < money)
                {
                    response.AddError("money","Not enough money.");
                }
                else
                {
                    Money -= money.GetValueOrDefault();   
                }
                foreach (var good in goods)
                {
                    response.Merge(BringGood(good));
                }
            }
            else
            {
                response.AddError("operation", "Not known operation");
            }

            response.Response = this;

            return response;
        }
    }
}