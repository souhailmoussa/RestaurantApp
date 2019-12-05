using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantApplication.Api.Common;

namespace RestaurantApplication.Api.Configuration
{
    public class DatabaseConfig : IDatabaseConfig
    {
        public string RestaurantConnectionString
        {
            get;
            set;
        }

        public void LoadConfiguration(IConfiguration configuration)
        {
            var section = configuration.GetSection(Constants.ConfigSections.ConnectionStrings);

            section?.Bind(this);
        }
    }
}
