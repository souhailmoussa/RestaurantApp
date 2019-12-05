using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApplication.Api.Configuration
{
    public interface IConfigService
    {
        T Get<T>(string sectionName) where T : class;
        string GetAll();
    }
}
