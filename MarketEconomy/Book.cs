using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common;

namespace MarketEconomy
{
    public class Book: object
    {
        public Book(string name)
        {
            Name = name;
            SortedAsks = new SortedList<double,Offer>(Comparer<double>.Create((x, y) => x == y ? 1 : x.CompareTo(y)));
            SortedBids = new SortedList<double,Offer>(Comparer<double>.Create((x, y) => y == x ? 1 : y.CompareTo(x)));
        }
        
        private SortedList<double,Offer> SortedAsks { get; set; }
        private SortedList<double,Offer> SortedBids { get; set; }
        
        public double Price { get; private set; }
        public string Name { get; set; }
        
        public List<Offer> Asks
        {
            get { return SortedAsks.Select(x => x.Value).ToList(); }
        }
        
        public List<Offer> Bids
        {
            get { return SortedBids.Select(x => x.Value).ToList(); }
        }

        public OperationResponse<Book> AddAsk(Offer ask)
        {
            SortedAsks.Add(ask.Price,ask);
            return new OperationResponse<Book>(){Response = this};
        }

        public OperationResponse<Book>  AddBid(Offer bid)
        {
            SortedBids.Add(bid.Price,bid);
            return new OperationResponse<Book>(){Response = this};
        }


        public OperationResponse<Book> Resolve()
        {
            var ask = SortedAsks.FirstOrDefault().Value;
            var bid = SortedBids.FirstOrDefault().Value;
            while (bid != null && ask != null && ask.Price <= bid.Price)
            {
                //TO.DO wydzielić jako oddzielny mechanizm
                var amount = Math.Min(ask.Amount, bid.Amount);
                var price = (bid.Price + ask.Price) / 2;
                var value = amount * price;
                Price = price;
                
                //TO.DO wydzielić jako oddzielny mechanizm
                ask.Amount -= amount;
                bid.Amount -= amount;
                
                //TO.DO wydzielić jako oddzielny mechanizm
                ask.Offerer.Money -= value;
                ask.Offerer.Goods[Name].Amount += amount;
                bid.Offerer.Money += value;
                bid.Offerer.Goods[Name].Amount -= amount;
                
                

                if (ask.Amount == 0)
                {
                    SortedAsks.RemoveAt(0);
                }

                if (bid.Amount == 0)
                {
                    SortedBids.RemoveAt(0);
                }
                
                ask = SortedAsks.FirstOrDefault().Value;
                bid = SortedBids.FirstOrDefault().Value;
                
            }
            
            return new OperationResponse<Book>(){Response = this};
        }
    }
    
    
}

