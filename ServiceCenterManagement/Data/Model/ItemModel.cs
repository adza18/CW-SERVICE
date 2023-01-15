using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterManagement.Data.Model
{
    public class ItemModel
    {
        //A class that holds the properties of item model
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public int Stock { get; set; }
        public string Desc { get; set; }
        public int Price { get; set; }
        public Guid AddedBy { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.Now;
        public DateTime? LastOrderedDate { get; set; }

    }
}
