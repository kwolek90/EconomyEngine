using System.Collections;

namespace MarketEconomy
{
    public class Offer
    {
        public Offer(Customer offerer, double price, double amount)
        {
            Offerer = offerer;
            Price = price;
            Amount = amount;
        }
        public double Price { get; set; }
        public double Amount { get; set; }
        
        public Customer Offerer { get; set; }
    }
}