using RestaurantApplication.Api.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApplication.Api.Models
{
    public class Table : ModelBase
    {
        public string TableNumber { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}
