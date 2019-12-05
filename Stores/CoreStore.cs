using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using RestaurantApplication.Api.Common;
using RestaurantApplication.Api.Configuration;
using RestaurantApplication.Api.Models;
using RestaurantApplication.Api.Mongo;

namespace RestaurantApplication.Api.Stores
{
    public class CoreStore : ICoreStore
    {
        private readonly MongoService mongoService;

        public CoreStore(IDatabaseConfig databaseConfig)
        {
            if(databaseConfig == null)
            {
                throw new NotImplementedException(nameof(databaseConfig));
            }
            this.mongoService = MongoService.GetService(databaseConfig.RestaurantConnectionString);
        }

        public async Task<IEnumerable<Table>> GetTables()
        {
            var tableCollection = mongoService.GetCollection<Table>(Constants.Collections.TablesCollection);

            var tables = await tableCollection.Find(m => true).ToListAsync();

            return tables;
        }
    }
}
