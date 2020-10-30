using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data.DTOs
{
    public class StatsByDate
    {
        public string YearMonth { get; set; }
        public string ProductName { get; set; }
        public double TotalSales { get; set; }

        public List<StatsByProduct> Products { get; set; }
    }
}
