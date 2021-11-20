using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            var connectionStr = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            var client = new MongoClient(connectionStr);

            string dbName = configuration.GetValue<string>("DatabaseSettings:DatabaseName");
            var database = client.GetDatabase(dbName);

            string collectionName = configuration.GetValue<string>("DatabaseSettings:CollectionName");
            Products = database.GetCollection<Product>(collectionName);

            CatalogContextSeed.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; }
    }
}
