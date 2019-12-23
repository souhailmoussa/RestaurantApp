using RestaurantApplication.Api.Common;
using RestaurantApplication.Api.Models;
using RestaurantApplication.Api.Stores;
using RestaurantApplication.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApplication.Api.Managers
{
    public class CoreManager
    {
        private readonly ICoreStore coreStore;

        public CoreManager( ICoreStore coreStore)
        {
            this.coreStore = coreStore ?? throw new ArgumentNullException(nameof(coreStore));
        }
        public async Task<IEnumerable<Table>> GetTables()
        {
            return await coreStore.GetTables();
        }

        public async Task<Table> GetTableById(string id)
        {
            return await coreStore.GetTableById(id);
        }

        public async Task<SubmissionResponse> SaveTable(Table table, bool isUpdate)
        {
            if (table == null)
            {
                return SubmissionResponse.Error();
            }

            if (isUpdate && table.Id.IsEmptyOrEmptyGuid())
            {
                return SubmissionResponse.Error(Constants.Errors.EmptyPayload);
            }

            if (isUpdate)
            {
                table.SetUpdateCommonFields();
            }
            else
            {
                if (table.Id.IsEmptyOrEmptyGuid())
                {
                    table.Id = Guid.NewGuid().ToString();
                }
                table.SetNewCommonFields();
            }

            return await coreStore.SaveTable(table, isUpdate);
        }
    }
}
