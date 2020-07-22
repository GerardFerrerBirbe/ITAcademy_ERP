using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Models
{
    public class OrderHeaderDTO
    {        
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public int AddressId { get; set; }
        public string Address { get; set; }
        public int ClientId { get; set; }
        public string Client { get; set; }
        public int EmployeeId { get; set; }
        public string Employee { get; set; }
        public int OrderStateId { get; set; }
        public string OrderState { get; set; }
        public int OrderPriorityId { get; set; }
        public string OrderPriority { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime AssignToEmployeeDate { get; set; }
        public DateTime FinalisationDate { get; set; }
        public ICollection<OrderLineDTO> OrderLines { get; set; }
    }
}
