﻿using ApiStocksLib;
using BagLib;
using BagLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BagMVC.Controllers
{
    public class _BaseController : Controller
    {
        protected readonly BagContext _context;

        protected int CurrentUserId { get; set; } = 1;

        protected IConfiguration _configuration;

        public _BaseController(BagContext context)
        {
            _context = context;
        }

        public _BaseController(BagContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //Metodo para llamar a la API
        protected ApiStocksLib.ApiStocksClient GetStockApiClient()
        {
            return new(_configuration["ApiStocksUrl"], _configuration["ApiKey"]);
        }

        protected async Task<Stock> CheckAndCreate(string ticket)
        {
            //1: Compruebo que existe
            Stock stock = await _context.GetStockAsync(ticket);

            if(stock == null)
            {
                //2: Si NO existe, lo busco en la API
                var stockClient = GetStockApiClient();

                var company = await stockClient.GetCompany(ticket);

                if(company != null)
                {
                    //3: Ver si está el mercado
                    var market = await _context.GetMarketAsync(company.Exchange);

                    if(market == null)
                    {   
                        Country country = await _context.GetCountryByNameAsync(company.Country);
                        Currency currency = await _context.GetCurrencyAsync(company.Currency);

                        market = new(company.Exchange, " ", country.CountryId, currency.CurrencyId);

                        market = await _context.InsertMarketAsync(market);
                    }
                    stock = new Stock(company.Name, company.Symbol, market.MarketId);

                    stock = await _context.InsertStockAsync(stock);
                }
            }
            return stock;
        }
    }
}
