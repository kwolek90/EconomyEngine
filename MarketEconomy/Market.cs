using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Common;

namespace MarketEconomy
{
    public class Market
    {
        public Market()
        {
            Books = new Dictionary<string,Book>();
            Customers = new Dictionary<string, Customer>();
        }
        private Dictionary<string,Book> Books { get; set; }
        private Dictionary<string, Customer> Customers { get; set; }
        
        public bool InstantResolve { get; set; }


        public OperationResponse<Book> GetBookByName(string name)
        {
            var response = new OperationResponse<Book>();
            Book book = null;
            if (string.IsNullOrWhiteSpace(name))
            {
                response.AddError("book",$"No book name provided.");
            }
            else
            {
                book = Books.GetValueOrDefault(name);
            }

            if (book == null)
            {
                response.AddError("book",$"Book {name} does not exists.");
            }

            response.Response = book;
            return response;
        }

        public OperationResponse CreateBook(string name)
        {
            var response = new OperationResponse();
            if (string.IsNullOrWhiteSpace(name))
            {
                response.AddError("book",$"No book name provided.");
            }
            else if (Books.ContainsKey(name))
            {
                response.AddError("name","Book already exists");
            }
            else
            {
                Books[name] = new Book(name);   
            }
            return response;
        }

        public OperationResponse<Customer> GetCustomerById(string id)
        {
            return new OperationResponse<Customer>(){Response = Customers.GetValueOrDefault(id)};
        }
        
        //TO.DO przenieść do jakiś utils czy helperów
        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        public OperationResponse<Customer> AddCustomer(string name, double money)
        {
            var customer = new Customer()
            {
                Name = name,
                Money = money,
                Id = RemoveDiacritics(name).Replace(' ', '_')
            };
            Customers[customer.Id] = customer;

            return new OperationResponse<Customer>() {Response = customer};
        }
        
        public OperationResponse AddAsk(string bookName, string customerId, double price, double amount)
        {
            var response = GetCustomerById(customerId)
                .NextStep(x => x.PrepareAsk(bookName, price, amount))
                .NextStep(x => GetBookByName(bookName).NextStep(y => y.AddAsk(x)))
                .ConditionalStep(x => InstantResolve, x => x.Resolve());
            return response;
        }
        public OperationResponse AddBid(string bookName, string customerId, double price, double amount)
        {
            var response = GetCustomerById(customerId)
                .NextStep(x => x.PrepareBid(bookName, price, amount))
                .NextStep(x => GetBookByName(bookName).NextStep(y => y.AddBid(x)))
                .ConditionalStep(x => InstantResolve, x => x.Resolve());
            return response;
        }

    }
}