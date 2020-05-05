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
            if (string.IsNullOrWhiteSpace(marketName) || string.IsNullOrWhiteSpace(bookName))
            {
                response.AddError("","No names provided");
                return response;
            }

            response = GetMarketByName(marketName).NextStep(x => x.GetBookByName(bookName));
            return response;
        }

        public OperationResponse CreateMarketBook(string marketName, string bookName)
        {
            OperationResponse response = null;
            if (string.IsNullOrWhiteSpace(bookName))
            {
                response = new OperationResponse();
                response.AddError("book",$"No book name provided.");
            }
            else
            {
                response = GetMarketByName(marketName).NextStep(x => x.CreateBook(bookName));
            }

            return response;
        }

        public OperationResponse AddAsk(string marketName, string bookName, string customerId, double price, double amount)
        {
            var marketResponse = GetMarketByName(marketName);
            var offerer = marketResponse.NextStep(x => x.GetCustomerById(customerId)).Response;
            var bookResposne = marketResponse.NextStep(x => x.GetBookByName(bookName)); 
            var offer = new Offer(offerer, price, amount);
            bookResposne.NextStep(x => x.AddAsk(offer));

            var response = marketResponse.NextStep(x =>
            {
                if (x.InstantResolve)
                {
                    bookResposne.NextStep(x => x.Resolve());
                }
                return new OperationResponse();
            });

            return response;
        }
        
        public OperationResponse AddBid(string marketName, string bookName, string customerId, double price, double amount)
        {
            var marketResponse = GetMarketByName(marketName);
            var offerer = marketResponse.NextStep(x => x.GetCustomerById(customerId)).Response;
            var book = marketResponse.NextStep(x => x.GetBookByName(bookName)).Response; 
            var offer = new Offer(offerer, price, amount);
            book.AddBid(offer);
            
            var response = marketResponse.NextStep(x =>
            {
                if (x.InstantResolve)
                {
                    book.Resolve();
                }
                return new OperationResponse();
            });

            return response;
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
        
        
        public OperationResponse<Customer> GetMarketCustomerById(string marketName, string id)
        {
            return GetMarketByName(marketName).NextStep(x => x.GetCustomerById(id));
        }

        public List<string> GetAllMarketsNames()
        {
            return Markets.Select(x => x.Key).ToList();
        }
    }
}