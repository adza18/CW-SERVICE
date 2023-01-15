using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterManagement.Data.Model
{
    public class RecordModel
    {
       //A class that holds the properties of record model
        public Guid Id { get; set; } = Guid.NewGuid();
        public int QuantityTakenOut { get; set; }
        public Guid ApprovedBy { get; set; }
        public Guid ItemId { get; set; }
        public bool IsApproved { get; set; } = false;

        public OrderStatus Status { get; set; }
        public Guid RequestedBy { get; set; }
        public DateTime OrderedDate { get; set; } = DateTime.Now;
        public DateTime ApprovedDate { get; set; }
        public int OrderTotal { get; set; }










    }
}
