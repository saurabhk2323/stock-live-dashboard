using System;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;

namespace StockLiveDashboard.Services.DatabaseServices
{
    public class CosmosDBClient
    {
        private readonly CosmosClient _client;
        public readonly Database _database;
        public CosmosDBClient()
        {
            CosmosClientOptions options = new CosmosClientOptions()
            {
                ConnectionMode = ConnectionMode.Direct
            };
            _client = new(
                "https://localhost:8081",
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
                );
            _database = _client.GetDatabase("stocklivedashboard");
        }
    }
}
