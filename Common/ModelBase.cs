using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApplication.Api.Common
{
    public class ModelBase
    {
        [BsonId]
        public string Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public void SetNewCommonFields(string createdBy = null, string modifiedBy = null)
        {
            if (createdBy.NotEmpty()) CreatedBy = createdBy;
            if (modifiedBy.NotEmpty()) ModifiedBy = modifiedBy;
            if (!CreatedOn.HasValue) CreatedOn = DateTime.UtcNow;
            if (!ModifiedOn.HasValue) ModifiedOn = DateTime.UtcNow;
        }

        public void SetUpdateCommonFields(string modifiedBy = null)
        {
            if (modifiedBy.NotEmpty()) ModifiedBy = modifiedBy;
            if (!ModifiedOn.HasValue) ModifiedOn = DateTime.UtcNow;
        }
    }
}
