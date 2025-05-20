using Microsoft.AspNetCore.SignalR;
using StockLiveDashboard.Contracts;
using System;
using System.Collections.Generic;


namespace StockLiveDashboard.Services.Hubs
{
    public class StockHub : Hub
    {
        private readonly IStocksService _stocksService;

        public StockHub(IStocksService stocksService)
        {
            _stocksService = stocksService;
        }

        public async Task BroadCastStocks()
        {
            var stocks = await _stocksService.GetAllStocks();
            await Clients.All.SendAsync("receivestocks", stocks);
        }
    }
}
