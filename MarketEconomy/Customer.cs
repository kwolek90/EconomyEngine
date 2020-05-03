using System.Collections.Generic;

namespace MarketEconomy
{
    public class Customer
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public double Money { get; set; }
        public double BlockedMoney { get; set; }
        
        public Dictionary<string,Good> Goods { get; set; }
        public Dictionary<string,Good> BlockedGoods { get; set; }
    }
}