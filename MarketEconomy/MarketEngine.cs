using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketEconomy
{
    public sealed class MarketEngine
    {
        private static readonly MarketEngine _instance = new MarketEngine();
        public static MarketEngine Instance
        {
            get { return _instance; }
        }

        private Dictionary<string,Market> Markets { get; set; }
        
        

        private MarketEngine()
        {
            Markets = new Dictionary<string,Market>();
        }

        public void CreateStockMarket(string name)
        {
            if (Markets.ContainsKey(name))
            {
                throw new Exception("Book exists");
            }
            Markets[name] = new Market(){InstantResolve = true};
        }

        public Market GetMarketByName(string name)
        {
            return Markets.GetValueOrDefault(name);
        }

        public Book GetMarketBookByName(string marketName, string bookName)
        {
            return GetMarketByName(marketName)?.GetBookByName(bookName);
        }

        public void CreateMarketBook(string marketName, string bookName)
        {
            var market = GetMarketByName(marketName);
            market.CreateBook(bookName);
        }

        public void AddAsk(string marketName, string bookName, string customerId, double price, double amount)
        {
            var offerer = GetMarketCustomerById(marketName, customerId);
            var offer = new Offer(offerer, price, amount);
            var market = GetMarketByName(marketName);
            var book = market.GetBookByName(bookName); 
            book.AddAsk(offer);
            if (market.InstantResolve)
            {
                book.Resolve();
            }
        }
        
        public void AddBid(string marketName, string bookName, string customerId, double price, double amount)
        {
            var offerer = GetMarketCustomerById(marketName, customerId);
            var offer = new Offer(offerer, price, amount);
            var market = GetMarketByName(marketName);
            var book = market.GetBookByName(bookName);
            book.AddBid(offer);
            if (market.InstantResolve)
            {
                book.Resolve();
            }
        }

        public Customer AddMarketCustomer(string marketName, string name, double money)
        {
            var market = GetMarketByName(marketName);
            return market.AddCustomer(name, money);
        }
        
        
        public Customer GetMarketCustomerById(string marketName, string id)
        {
            return GetMarketByName(marketName)?.GetCustomerById(id);
        }

        public List<string> GetAllMarketsNames()
        {
            return Markets.Select(x => x.Key).ToList();
        }
    }
}