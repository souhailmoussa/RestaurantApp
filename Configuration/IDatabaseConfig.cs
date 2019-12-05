using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApplication.Api.Configuration
{
    public interface IDatabaseConfig : IConfig
    {
        string RestaurantConnectionString { get; }
    }
}
