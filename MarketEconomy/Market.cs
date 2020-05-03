using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MarketEconomy
{
    public class Market
    {
        public Market()
        {
            Books = new Dictionary<string,Book>();
        }
        private Dictionary<string,Book> Books { get; set; }
        private Dictionary<string, Customer> Customers { get; set; }
        
        public bool InstantResolve { get; set; }


        public Book GetBookByName(string name)
        {
            return Books.GetValueOrDefault(name);
        }

        public void CreateBook(string name)
        {
            if (Books.ContainsKey(name))
            {
                throw new Exception("Book exists");
            }
            Books[name] = new Book();
        }

        public Customer GetCustomerById(string id)
        {
            return Customers.GetValueOrDefault(id);
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

        public Customer AddCustomer(string name, double money)
        {
            var customer = new Customer()
            {
                Name = name,
                Money = money,
                Id = RemoveDiacritics(name).Replace(' ', '_')
            };
            Customers[customer.Id] = customer;

            return customer;
        }

    }
}