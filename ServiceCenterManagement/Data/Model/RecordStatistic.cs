using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterManagement.Data.Model
{
    public class RecordStatistic
    {
        //A class that holds the properties of RecordStatistic model to map the relationship between an item and its quantity ordered
        public Guid ItemId { get; set; }
        public int ItemQuantity { get; set; }

    }
}
