using System;
using System.Collections.Generic;
using MarketEconomy;
using NUnit.Framework;

namespace MarketTests
{
    public class Tests
    {
        string marketName = "market";
        string bookName = "grain";
        string goodName
        {
            get { return bookName; }
        }
        string customerName = "customer";

        private void InitializeMarket()
        {
            MarketEngine.Instance.DeleteMarket(marketName);
            MarketEngine.Instance.CreateMarket(marketName);
            MarketEngine.Instance.AddMarketCustomer(marketName, customerName,100);
            MarketEngine.Instance.CreateMarketBook(marketName, bookName);   
        }
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AddAsk()
        {
            InitializeMarket();
            var price = 11;
            var amount = 11;
            var response = MarketEngine.Instance.AddAsk(marketName, bookName, customerName,price, amount);
            Assert.IsFalse(response.Success);
        }
        [Test]
        public void AddBid()
        {
            InitializeMarket();
            var price = 11;
            var amount = 11;
            var response = MarketEngine.Instance.AddBid(marketName, bookName, customerName,price,amount);
            Assert.IsFalse(response.Success);
        }

        [Test]
        public void AddBidDetailed()
        {
            InitializeMarket();
            var price = 11;
            var amount = 11;
            var market = MarketEngine.Instance.GetMarketByName(marketName).Response;
            
            
            
            var customer = market.GetCustomerById(customerName);
            var offer =    customer.NextStep(x => x.PrepareBid(bookName, price, amount));
            Assert.IsFalse(offer.Success);
            var book  =  offer.NextStep(x => market.GetBookByName(bookName).NextStep(y => y.AddBid(x)));
            Assert.IsFalse(book.Success);
            var response = book.ConditionalStep(x => market.InstantResolve, x => x.Resolve());
            Assert.IsFalse(response.Success);
        }
        
        [Test]
        public void GetCustomer()
        {
            InitializeMarket();
            var customer = MarketEngine.Instance.GetMarketCustomerById(marketName, customerName);
            Assert.IsTrue(customer.Success);
        }
    }
}