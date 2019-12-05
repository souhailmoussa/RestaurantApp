using RestaurantApplication.Api.Models;
using RestaurantApplication.Api.Stores;
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
    }
}
