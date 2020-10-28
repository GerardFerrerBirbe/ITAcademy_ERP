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

        public List<ProductDTO> GetTopProducts()
        {            
            var products = _context.OrderLines
                    .Include(o => o.Product)
                    .GroupBy(o => o.Product.ProductName)
                    .Select(o => new ProductDTO {
                        ProductName = o.Key, 
                        TotalSales = o.Sum(o => o.UnitPrice * (1 + o.Vat) * o.Quantity)})
                    .OrderByDescending(x => x.TotalSales).Take(3)
                    .ToList();

            return products;
        }

        public List<ClientDTO> GetTopClients()
        {
            var clients = _context.OrderLines
                    .Include(o => o.OrderHeader)
                    .ThenInclude(o => o.Client)
                    .ThenInclude(c => c.Person)
                    .GroupBy(o => o.OrderHeader.Client.Person.Email)
                    .Select(g => new ClientDTO
                    {
                        Email = g.Key,
                        FirstName = _context.People.FirstOrDefault(p => p.Email == g.Key).FirstName,
                        LastName = _context.People.FirstOrDefault(p => p.Email == g.Key).LastName,
                        TotalSales = g.Sum(o => o.UnitPrice * (1 + o.Vat) * o.Quantity)
                    })
                    .OrderByDescending(x => x.TotalSales).Take(3)
                    .ToList();

            return clients;
        }

        public List<OrderHeaderDTO> GetSalesEvolution()
        {
            var orderHeaders = _context.OrderLines
                    .Include(o => o.OrderHeader)
                    .GroupBy(o => new { month = o.OrderHeader.CreationDate.Month, year = o.OrderHeader.CreationDate.Year })
                    .Select(g => new OrderHeaderDTO
                    {
                        YearMonth = g.Key.year.ToString() + "-" + g.Key.month.ToString(),
                        TotalSales = g.Sum(o => o.UnitPrice * (1 + o.Vat) * o.Quantity)
                    })                    
                    .ToList();

            return orderHeaders;
        }
    }
}
