using ITAcademyERP.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data
{
    public class DummyData
    {
        public static void Initialize(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var _context = serviceScope.ServiceProvider.GetService<ITAcademyERPContext>();
            _context.Database.EnsureCreated();


            // Look for any users
            if (_context.ProductCategories.Count() != 0)
                return;

            var people = GetPeople().ToArray();
            _context.People.AddRange(people);
            _context.SaveChanges();

            var clients = GetClients().ToArray();
            _context.Clients.AddRange(clients);
            _context.SaveChanges();

            var employees = GetEmployees().ToArray();
            _context.Employees.AddRange(employees);
            _context.SaveChanges();

            var addresses = GetAddresses().ToArray();
            _context.Addresses.AddRange(addresses);
            _context.SaveChanges();

            var productCategories = GetProductCategories().ToArray();
            _context.ProductCategories.AddRange(productCategories);
            _context.SaveChanges();

            var products = GetProducts().ToArray();
            _context.Products.AddRange(products);
            _context.SaveChanges();

            var orderStates = GetOrderStates().ToArray();
            _context.OrderStates.AddRange(orderStates);
            _context.SaveChanges();

            var orderPriorities = GetOrderPriorities().ToArray();
            _context.OrderPriorities.AddRange(orderPriorities);
            _context.SaveChanges();

            var orderHeaders = GetOrderHeaders().ToArray();
            _context.OrderHeaders.AddRange(orderHeaders);
            _context.SaveChanges();

            var orderLines = GetOrderLines().ToArray();
            _context.OrderLines.AddRange(orderLines);
            _context.SaveChanges();

            var role = "Admin";
            var roleStore = new RoleStore<IdentityRole>(_context);
            roleStore.CreateAsync(new IdentityRole(role));
        }
        //    var user = new ApplicationUser
        //    {
        //        Email = "proves@e.com",
        //        NormalizedEmail = "PROVES@E.COM",
        //        UserName = "Admin",
        //        NormalizedUserName = "ADMIN",
        //        EmailConfirmed = true,
        //        PhoneNumberConfirmed = true,
        //        SecurityStamp = Guid.NewGuid().ToString("D")
        //    };

        //    if (!_context.Users.Any(u => u.UserName == user.UserName))
        //    {
        //        var password = new PasswordHasher<ApplicationUser>();
        //        var hashed = password.HashPassword(user, "Aa111111!");
        //        user.PasswordHash = hashed;

        //        var userStore = new UserStore<ApplicationUser>(_context);
        //        var result = userStore.CreateAsync(user);
        //    }

        //    _context.SaveChangesAsync();

        //    AssignRoles(serviceProvider, user.Email, role).Wait();

        //    _context.SaveChangesAsync();
        //}

        //public static async Task<IdentityResult> AssignRoles(IServiceProvider services, string email, string role)
        //{
        //    UserManager<ApplicationUser> _userManager = services.GetService<UserManager<ApplicationUser>>();
        //    ApplicationUser user = await _userManager.FindByEmailAsync(email);
        //    var result = await _userManager.AddToRoleAsync(user, role);

        //    return result;
        //}

        public static List<Person> GetPeople()
        {
            List<Person> people = new List<Person>()
            {
                new Person{Email = "gf@example.com", FirstName = "Gerard", LastName = "Ferrer Birbe"},
                new Person{Email = "mg@example.com", FirstName = "Maria", LastName = "Gonzalez Martí"},
                new Person{Email = "js@example.com", FirstName = "Joanna", LastName = "Solé Carrasco"},
                new Person{Email = "jm@example.com", FirstName = "Josep", LastName = "Martinez Teixidó"}
            };
            return people;
        }
        public static List<Address> GetAddresses()
        {
            List<Address> addresses = new List<Address>()
            {
                new Address{Name = "C/ Montjuic, 127", Type = "Personal"},
                new Address{Name = "C/ Barcelona, 328", Type = "Delivery"},
                new Address{Name = "C/ Perill, 12", Type = "Personal"},
                new Address{Name = "C/ Girona, 124", Type = "Delivery"}
            };
            return addresses;
        }
        public static List<ProductCategory> GetProductCategories()
        {
            List<ProductCategory> productCategories = new List<ProductCategory>()
            {
                new ProductCategory{ProductCategoryName = "Bicicletes"},
                new ProductCategory{ProductCategoryName = "Motos"},
                new ProductCategory{ProductCategoryName = "Cotxes"}
            };
            return productCategories;
        }
        public static List<Product> GetProducts()
        {
            List<Product> products = new List<Product>()
            {
                new Product{ProductName = "Trek", ProductCategoryId = 1},
                new Product{ProductName = "Montesa", ProductCategoryId = 2},
                new Product{ProductName = "Fiat", ProductCategoryId = 3}
            };
            return products;
        }
        
        public static List<Client> GetClients()
        {
            List<Client> clients = new List<Client>()
            {
            };
            return clients;
        }
        public static List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>()
            {
                new Employee{Position = "Director", Salary = 20000},
                new Employee{Position = "Manager", Salary = 15000}
            };
            return employees;
        }
        public static List<OrderState> GetOrderStates()
        {
            List<OrderState> orderStates = new List<OrderState>()
            {
                new OrderState{State = "Pendent de tractar"},
                new OrderState{State = "En tractament"},
                new OrderState{State = "En repartiment"},
                new OrderState{State = "Complet"},
                new OrderState{State = "Cancel·lat"}
            };
            return orderStates;
        }
        public static List<OrderPriority> GetOrderPriorities()
        {
            List<OrderPriority> orderPriorities = new List<OrderPriority>()
            {
                new OrderPriority{Priority = "Baixa"},
                new OrderPriority{Priority = "Mitjana"},
                new OrderPriority{Priority = "Alta"}
            };
            return orderPriorities;
        }
        public static List<OrderHeader> GetOrderHeaders()
        {
            List<OrderHeader> orderHeaders = new List<OrderHeader>()
            {
                new OrderHeader
                {
                    OrderNumber = "2020-01",
                    EmployeeId = 1,
                    OrderPriorityId = 1,
                    OrderStateId = 3,
                    CreationDate = new DateTime(2020,09,09),
                    AssignToEmployeeDate = new DateTime(2020,09,09),
                    FinalisationDate = new DateTime(2020,09,09)
                },
                new OrderHeader
                {
                    OrderNumber = "2020-02",
                    EmployeeId = 1,
                    OrderPriorityId = 3,
                    OrderStateId = 2,
                    CreationDate = new DateTime(2020,09,09),
                    AssignToEmployeeDate = new DateTime(2020,09,09),
                    FinalisationDate = new DateTime(2020,09,09)
                },
                new OrderHeader
                {
                    OrderNumber = "2020-03",
                    EmployeeId = 2,
                    OrderPriorityId = 2,
                    OrderStateId = 1,
                    CreationDate = new DateTime(2020,09,09),
                    AssignToEmployeeDate = new DateTime(2020,09,09),
                    FinalisationDate = new DateTime(2020,09,09)
                }
            };
            return orderHeaders;
        }
        public static List<OrderLine> GetOrderLines()
        {
            List<OrderLine> orderLines = new List<OrderLine>()
            {
                new OrderLine{ProductId = 1, UnitPrice = 100, Vat = 20, Quantity = 3, OrderHeaderId = 1},
                new OrderLine{ProductId = 2, UnitPrice = 200, Vat = 40, Quantity = 5, OrderHeaderId = 1},
                new OrderLine{ProductId = 3, UnitPrice = 300, Vat = 60, Quantity = 8, OrderHeaderId = 1}
            };
            return orderLines;
        }

    }        
}
