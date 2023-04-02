using System;
using Azure;
using Azure.Data.Tables;

namespace ExperisVinlotteri.Models
{
	public class User_DataBaseEntry : ITableEntity
    {
      
        public User_DataBaseEntry(string guid)
        {
            PartitionKey = "Vinlotteri_Partition";
            RowKey = guid;
            GUID = guid;
        }

        public User_DataBaseEntry()
        {

        }


        public string GUID { get; set; }
        public string? Name { get; set; }
        public int? SelectedNumber { get; set; }

        // Interface Properties
        
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    
	}
}

