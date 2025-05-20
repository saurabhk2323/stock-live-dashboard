using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using StockLiveDashboard.Contracts.Enums;
using System.Linq.Expressions;
using System.Net;

namespace StockLiveDashboard.Services.DatabaseServices
{
    public sealed class CosmosDBService
    {
        private readonly CosmosDBClient _cosmosDBClient;
        private readonly Database _database;

        public CosmosDBService(CosmosDBClient cosmosDBClient)
        {
            _cosmosDBClient = cosmosDBClient;
            _database = _cosmosDBClient._database;
        }

        public async Task<T> CreateItem<T>(Container container, T item, string? partitionKey) where T : class
        {
            var response = await container.CreateItemAsync<T>(item, new PartitionKey(partitionKey));
            return response.Resource;
        }

        public async Task<bool> DeleteItem<T>(Container container, string id, string partitionKey)
        {
            try
            {
                await container.DeleteItemAsync<T>(id, new PartitionKey(partitionKey));
                return true;
            }
            catch (CosmosException cex) when (cex.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
        }

        private async Task<List<T>> ExecuteQueryAsync<T>(
            Container container,
            Expression<Func<T, bool>> predicate,
            string? partitionKey) where T : class
        {
            var query = container.GetItemLinqQueryable<T>(
                allowSynchronousQueryExecution: true,
                requestOptions: string.IsNullOrWhiteSpace(partitionKey)
                                        ? new QueryRequestOptions { MaxItemCount = -1, MaxConcurrency = -1 }
                                        : new QueryRequestOptions { MaxItemCount = -1, PartitionKey = new PartitionKey(partitionKey) }
                );
            List<T> results = new();
            using (var iterator = query.Where(predicate).ToFeedIterator<T>())
            {
                while (iterator.HasMoreResults)
                {
                    FeedResponse<T> response = await iterator.ReadNextAsync();
                    results.AddRange(response);
                }
            }
            return results;
        }

        public Container GetContainer(ContainerTypes containerType) => _database.GetContainer(containerType.ToString());

        public async Task<T?> GetItem<T>(Container container, string id, string partitionKey) where T : class
        {
            try
            {
                var response = await container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (CosmosException cex) when (cex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<List<T>> GetItems<T>(
            Container container,
            Expression<Func<T, bool>> predicate,
            string partitionKey = "") where T : class => await ExecuteQueryAsync(container, predicate, partitionKey);

        public async Task<T> UpdateItem<T>(Container container, T item, string? partitionKey) where T : class
        {
            var response = await container.UpsertItemAsync<T>(item, new PartitionKey(partitionKey));
            return response.Resource;
        }
    }
}
