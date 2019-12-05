using RestaurantApplication.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApplication.Api.Stores
{
    public interface ICoreStore
    {
        Task<IEnumerable<Table>> GetTables();
    }
}
