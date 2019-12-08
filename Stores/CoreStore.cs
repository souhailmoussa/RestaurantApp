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
            if (databaseConfig == null)
            {
                throw new NotImplementedException(nameof(databaseConfig));
            }
            this.mongoService = MongoService.GetService(databaseConfig.RestaurantConnectionString);
        }

        public async Task<IEnumerable<Table>> GetTables()
        {
            return await mongoService.GetAllRecords<Table>(Constants.Collections.TablesCollection);
        }

        public async Task<Table> GetTableById(string tableId)
        {
            var tableCollection = mongoService.GetCollection<Table>(Constants.Collections.TablesCollection);

            var table = await tableCollection.Find(m => m.Id == tableId).FirstOrDefaultAsync();

            return table;
        }

        public async Task<SubmissionResponse> SaveTable(Table table, bool isUpdate)
        {
            var tableCollection = mongoService.GetCollection<Table>(Constants.Collections.TablesCollection);

            var update = Builders<Table>.Update
                .Set(m => m.ModifiedOn, table.ModifiedOn)
                .Set(m => m.ModifiedBy, table.ModifiedBy)
                .SetIfNotEmpty(m => m.TableNumber, table.TableNumber);

            if (!isUpdate)
            {
                update = update.SetIfNotEmpty(m => m.CreatedOn, table.CreatedOn)
                    .SetIfNotEmpty(m => m.CreatedBy, table.CreatedBy);
            }

            await tableCollection.UpdateOneAsync(table.Id.ToIdFilter<Table>(), update, new UpdateOptions { IsUpsert = true });

            return SubmissionResponse.Ok(table);

        }
    }
}
