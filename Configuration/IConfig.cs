using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApplication.Api.Configuration
{
    public interface IConfig
    {
        /// <summary>
        /// Load conifguration from configuration section.
        /// </summary>
        /// <param name="configuration"></param>
        void LoadConfiguration(IConfiguration configuration);
    }
}
