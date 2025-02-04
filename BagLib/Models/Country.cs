﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagLib.Models
{
    //public enum CurrencyType { Dolar, Euro, Pound, Jen, Xian, Looni }

    //public class Currency
    //{
    //    public Currency(int id, string name)
    //    {
    //        Id = id;
    //        Name = name;
    //    }

    //    public int Id { get; set; }
    //    public string Name { get; set; }

    //}

    public class Country
    {
        public Country()
        {
            Markets = new HashSet<Market>();
            Currencies = new HashSet<Currency>();
        }
        public Country(string name)
        {
            Name = name;
            Currencies = new HashSet<Currency>();
            Markets = new HashSet<Market>();
        }

        public int CountryId { get; set; }

        public string Name { get; set; }
        
        public string ApiName { get; set; }

        public ICollection<Market> Markets { get; set; }

        public ICollection<Currency> Currencies { get; set; }
    }
}
