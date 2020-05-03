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

        public OperationResponse CreateStockMarket(string name)
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

        public OperationResponse<Market> GetMarketByName(string name)
        {
            var response = new OperationResponse<Market>();
            Market market = null;
            if (string.IsNullOrWhiteSpace(name))
            {
                response.AddError("market",$"No market name provided.");
            }
            else
            {
                market = Markets.GetValueOrDefault(name);   
            }
            if (market == null)
            {
                response.AddError("market",$"Market {name} does not exists.");
            }

            response.Response = market;
            return response;
        }

        public OperationResponse<Book> GetMarketBookByName(string marketName, string bookName)
        {
            var response = new OperationResponse<Book>();
            var marketResposne = GetMarketByName(marketName);
            if (marketResposne.Success)
            {
                response = marketResposne.Response.GetBookByName(bookName);   
            }
            else
            {
                response.Merge(marketResposne);
            }
            return response;
        }

        public OperationResponse CreateMarketBook(string marketName, string bookName)
        {
            var response = GetMarketByName(marketName);
            if (response.Success)
            {
                if (string.IsNullOrWhiteSpace(bookName))
                {
                    response.AddError("book",$"No book name provided.");
                }
                else
                {
                    response.Merge(response.Response.CreateBook(bookName));   
                }
            }

            return response;
        }

        public void AddAsk(string marketName, string bookName, string customerId, double price, double amount)
        {
            var offerer = GetMarketCustomerById(marketName, customerId);
            var offer = new Offer(offerer, price, amount);
            var market = GetMarketByName(marketName).Response;
            var book = market.GetBookByName(bookName).Response; 
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
            var market = GetMarketByName(marketName).Response;
            var book = market.GetBookByName(bookName).Response;
            book.AddBid(offer);
            if (market.InstantResolve)
            {
                book.Resolve();
            }
        }

        public OperationResponse<Customer> AddMarketCustomer(string marketName, string name, double money)
        {
            var response = new OperationResponse<Customer>();
            var marketResponse = GetMarketByName(marketName); 
            response.Merge(marketResponse);
            if (response.Success)
            {
                response.Response = marketResponse.Response.AddCustomer(name, money);
            }
            return response;
        }
        
        
        public Customer GetMarketCustomerById(string marketName, string id)
        {
            return GetMarketByName(marketName).Response?.GetCustomerById(id);
        }

        public List<string> GetAllMarketsNames()
        {
            return Markets.Select(x => x.Key).ToList();
        }
    }
}