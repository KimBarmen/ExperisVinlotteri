using System;
using Azure.Data.Tables;
using ExperisVinlotteri.Models;

namespace ExperisVinlotteri.Services
{
	public interface ITableStorageService
	{

       
        Task<User_DataBaseEntry> GetEntityAsync(string category, string id);
        Task<User_DataBaseEntry> UpsertEntityAsync(User_DataBaseEntry entity);
        Task DeleteEntityAsync(string category, string id);
        Task<IEnumerable<User_DataBaseEntry>> GetAllAsync();
    }
}

