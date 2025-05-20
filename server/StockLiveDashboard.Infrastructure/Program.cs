using Microsoft.Azure.Cosmos;
using StockLiveDashboard.Infrastructure.MasterData;

public class Program
{
    private static CosmosClient _client;
    private static Database _database;

    public static async Task Main(string[] args)
    {
        Console.WriteLine("Creating Infrastructure.");
        Console.WriteLine("Create database");
        CosmosClientOptions options = new CosmosClientOptions()
        {
            ConnectionMode = ConnectionMode.Direct
        };
        _client = new(
            "https://localhost:8081",
            "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
            );
        _database = await _client.CreateDatabaseIfNotExistsAsync("stocklivedashboard");
        await CreateContainers();
        await LoadMasterDataINDB();
    }

    public static async Task CreateContainers()
    {
        Console.WriteLine("stocks container created");
        await _database.CreateContainerIfNotExistsAsync("investors", "/country_code");
        Console.WriteLine($"Create containers");
        await _database.CreateContainerIfNotExistsAsync("stocks", "/sector");
        Console.WriteLine("investors container created");
        await _database.CreateContainerIfNotExistsAsync("trades", "/investor_id");
        Console.WriteLine("trades container created");
    }

    public static async Task LoadMasterDataINDB()
    {
        // load data into investors container
        Container investorContainer = _database.GetContainer("investors");
        foreach(var item in LoadMasterData.GetInvestorList())
            await investorContainer.UpsertItemAsync(item, new PartitionKey(item.CountryCode));

        // laod data into stocks container
        Container stockContainer = _database.GetContainer("stocks");
        foreach (var item in LoadMasterData.GetStockList())
            await stockContainer.UpsertItemAsync(item, new PartitionKey(item.Sector));

        // laod data into trades container
        Container tradesContainer = _database.GetContainer("trades");
        foreach (var item in LoadMasterData.GetTradeList())
            await tradesContainer.UpsertItemAsync(item, new PartitionKey(item.InvestorID.ToString()));
    }
}