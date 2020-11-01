using ITAcademyERP.Data.DTOs;
using ITAcademyERP.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data.Repositories
{
    public class OrderLinesRepository : GenericRepository<Guid, OrderLine, ITAcademyERPContext>
    {
        private readonly ITAcademyERPContext _context;

        public OrderLinesRepository(ITAcademyERPContext context) : base(context)
        {
            _context = context;
        }
        public override async Task<List<OrderLine>> GetAll()
        {
            return await _context.OrderLines
                    .Include(o => o.Product)
                    .ToListAsync();
        }

        public List<StatsByProduct> GetTopProducts()
        {            
            var output = _context.OrderLines
                    .Include(o => o.Product)
                    .GroupBy(o => o.Product.ProductName)
                    .Select(o => new StatsByProduct {
                        ProductName = o.Key, 
                        TotalSales = o.Sum(o => o.UnitPrice * (1 + o.Vat) * o.Quantity)})
                    .OrderByDescending(x => x.TotalSales).Take(3)
                    .ToList();

            return output;
        }

        public List<StatsByProduct> GetSalesByProduct()
        {
            var output = _context.OrderLines
                    .Include(o => o.Product)
                    .GroupBy(o => o.Product.ProductName)
                    .Select(o => new StatsByProduct
                    {
                        ProductName = o.Key,
                        TotalSales = o.Sum(o => o.UnitPrice * (1 + o.Vat) * o.Quantity)
                    })
                    .OrderByDescending(x => x.TotalSales)
                    .ToList();

            return output;
        }

        public List<StatsByClient> GetTopClients()
        {
            var output = _context.OrderLines
                    .Include(o => o.OrderHeader)
                    .ThenInclude(o => o.Client)
                    .ThenInclude(c => c.Person)
                    .GroupBy(o => o.OrderHeader.Client.Person.Email)
                    .Select(g => new StatsByClient
                    {
                        Email = g.Key,
                        FirstName = _context.People.FirstOrDefault(p => p.Email == g.Key).FirstName,
                        LastName = _context.People.FirstOrDefault(p => p.Email == g.Key).LastName,
                        TotalSales = g.Sum(o => o.UnitPrice * (1 + o.Vat) * o.Quantity)
                    })
                    .OrderByDescending(x => x.TotalSales).Take(3)
                    .ToList();

            return output;
        }

        public List<StatsByClient> GetSalesByClient()
        {
            var output = _context.OrderLines
                    .Include(o => o.OrderHeader)
                    .ThenInclude(o => o.Client)
                    .ThenInclude(c => c.Person)
                    .GroupBy(o => o.OrderHeader.Client.Person.Email)
                    .Select(g => new StatsByClient
                    {
                        Email = g.Key,
                        FirstName = _context.People.FirstOrDefault(p => p.Email == g.Key).FirstName,
                        LastName = _context.People.FirstOrDefault(p => p.Email == g.Key).LastName,
                        TotalSales = g.Sum(o => o.UnitPrice * (1 + o.Vat) * o.Quantity)
                    })
                    .OrderByDescending(x => x.TotalSales)
                    .ToList();

            return output;
        }

        public List<StatsByDate> GetSalesByDate(string initialDate, string finalDate)
        {
            var initialYear = int.Parse(initialDate.Split("-")[0]);
            var initialMonth = int.Parse(initialDate.Split("-")[1]);
            var finalYear = int.Parse(finalDate.Split("-")[0]);
            var finalMonth = int.Parse(finalDate.Split("-")[1]);

            var initialDateTime = new DateTime(initialYear, initialMonth, 1);
            var finalDateTime = new DateTime(finalYear, finalMonth, DateTime.DaysInMonth(finalYear, finalMonth));

            var output = _context.OrderLines
                    .Include(o => o.OrderHeader)
                    .Where(o =>
                        o.OrderHeader.CreationDate >= initialDateTime &&
                        o.OrderHeader.CreationDate <= finalDateTime
                        )
                    .GroupBy(o => new { month = o.OrderHeader.CreationDate.Month, year = o.OrderHeader.CreationDate.Year })
                    .Select(g => new StatsByDate
                    {
                        YearMonth = g.Key.year.ToString() + "-" + g.Key.month.ToString(),
                        TotalSales = g.Sum(o => o.UnitPrice * (1 + o.Vat) * o.Quantity)
                    })                    
                    .ToList();

            return output;
        }

        public List<StatsByDate> GetSalesByDateAndProduct(string initialDate, string finalDate, string productName)
        {
            var initialYear = int.Parse(initialDate.Split("-")[0]);
            var initialMonth = int.Parse(initialDate.Split("-")[1]);
            var finalYear = int.Parse(finalDate.Split("-")[0]);
            var finalMonth = int.Parse(finalDate.Split("-")[1]);

            var initialDateTime = new DateTime(initialYear, initialMonth, 1);
            var finalDateTime = new DateTime(finalYear, finalMonth, DateTime.DaysInMonth(finalYear, finalMonth));

            var output = _context.OrderLines
                    .Include(o => o.OrderHeader)
                    .Include(o => o.Product)
                    .Where(o =>
                        o.OrderHeader.CreationDate >= initialDateTime &&
                        o.OrderHeader.CreationDate <= finalDateTime &&
                        o.Product.ProductName == productName
                        )
                    .GroupBy(o => new
                    {
                        Month = o.OrderHeader.CreationDate.Month,
                        Year = o.OrderHeader.CreationDate.Year,
                        ProductName = o.Product.ProductName
                    })
                    .Select(g => new StatsByDate
                    {
                        YearMonth = g.Key.Year.ToString() + "-" + g.Key.Month.ToString(),
                        ProductName = g.Key.ProductName,
                        TotalSales = g.Sum(o => o.UnitPrice * (1 + o.Vat) * o.Quantity)
                    })
                    .ToList();

            return output;            
        }
    }
}
