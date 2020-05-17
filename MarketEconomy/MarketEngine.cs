using System;
using System.Collections.Generic;
using System.Linq;
using Common;

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

        public OperationResponse CreateMarket(string name)
        {
            var response = new OperationResponse();
            if (Markets.ContainsKey(name))
            {
                response.AddError("name","Market already exists");
            }
            else
            {
                Markets[name] = new Market(){InstantResolve = true};   
            }
            return response;
        }
        
        
        public OperationResponse DeleteMarket(string name)
        {
            var response = new OperationResponse();
            if (!Markets.ContainsKey(name))
            {
                response.AddError("name","No such market");
            }
            else
            {
                Markets.Remove(name);
            }
            return response;
        }

        public OperationResponse<Market> GetMarketByName(string name)
        {
            var response = new OperationResponse<Market>();
            Market market = null;
            if (string.IsNullOrWhiteSpace(name))
            {
                response.AddError("market",$"No market name provided.");
                return response;
            }
            
            market = Markets.GetValueOrDefault(name);
            if (market == null)
            {
                response.AddError("market",$"Market {name} does not exists.");
            }

            response.Response = market;
            return response;
        }

        public OperationResponse<Book> GetMarketBookByName(string marketName, string bookName)
        {
            return GetMarketByName(marketName).NextStep(x => x.GetBookByName(bookName));
        }

        public OperationResponse CreateMarketBook(string marketName, string bookName)
        {
            return GetMarketByName(marketName).NextStep(x => x.CreateBook(bookName));
        }

        public OperationResponse AddAsk(string marketName, string bookName, string customerId, double price, double amount)
        {
            return GetMarketByName(marketName).NextStep(x => x.AddAsk(bookName, customerId, price, amount));
        }
        
        public OperationResponse AddBid(string marketName, string bookName, string customerId, double price, double amount)
        {
            return GetMarketByName(marketName).NextStep(x => x.AddBid(bookName, customerId, price, amount));
        }

        public OperationResponse<Customer> AddMarketCustomer(string marketName, string name, double money)
        {
            return GetMarketByName(marketName).NextStep(m => m.AddCustomer(name,money));
        }
        
        
        public OperationResponse<Customer> GetMarketCustomerById(string marketName, string id)
        {
            return GetMarketByName(marketName).NextStep(x => x.GetCustomerById(id));
        }
        
        
        public OperationResponse<Customer> UpdateMarketCustomer(string marketName, string id, string operation, double? money, List<Good> goods)
        {
            return GetMarketByName(marketName).NextStep(m => m.GetCustomerById(id))
                .NextStep(c => c.ModifyBelongings(operation, money, goods));
        }

        public List<string> GetAllMarketsNames()
        {
            return Markets.Select(x => x.Key).ToList();
        }
    }
}