using System;
using Azure.Data.Tables;
using ExperisVinlotteri.Models;

namespace ExperisVinlotteri.Services
{
	public class TableStorageService : ITableStorageService
    {
        private const string TableName = "Vinlotteri";
        private readonly IConfiguration _configuration;


        public TableStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        private async Task<TableClient> GetTableClient()
        {
            var serviceClient = new TableServiceClient(_configuration["StorageConnectionString"]);
            var tableClient = serviceClient.GetTableClient(TableName);
            await tableClient.CreateIfNotExistsAsync();
            return tableClient;
        }


        public async Task<User_DataBaseEntry> GetEntityAsync(string category, string id)
        {
            var tableClient = await GetTableClient();
            return await tableClient.GetEntityAsync<User_DataBaseEntry>(category, id);
        }


        public async Task<User_DataBaseEntry> UpsertEntityAsync(User_DataBaseEntry entity)
        {
            var tableClient = await GetTableClient();
            await tableClient.UpsertEntityAsync(entity);
            return entity;
        }


        public async Task DeleteEntityAsync(string category, string id)
        {
            var tableClient = await GetTableClient();
            await tableClient.DeleteEntityAsync(category, id);
        }


        public async Task<IEnumerable<User_DataBaseEntry>> GetAllAsync()
        {
            IList<User_DataBaseEntry> modelList = new List<User_DataBaseEntry>();
            var tableClient = await GetTableClient();

            var celebs = tableClient.QueryAsync<User_DataBaseEntry>(filter: "", maxPerPage: 1000);
            await foreach (var celeb in celebs)
            {
                modelList.Add(celeb);
            }
            return modelList;
        }

    }
}

