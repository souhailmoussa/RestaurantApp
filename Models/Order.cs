using RestaurantApplication.Api.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApplication.Api.Models
{
    public class Order : ModelBase
    {
        public string Name { get; set; }

        public string TableId { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
