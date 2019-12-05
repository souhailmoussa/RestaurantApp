using RestaurantApplication.Api.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApplication.Api.Models
{
    public class Product : ModelBase
    {
        /// <summary>
        /// Represent the arabic name of the product
        /// </summary>
        public string NameAr { get; set; }

        /// <summary>
        /// Represent the english name of the product
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// Represent the price of the product
        /// </summary>
        public decimal Price { get; set; }
    }
}
