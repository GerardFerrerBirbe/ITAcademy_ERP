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
            var _roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
            var _userManager = serviceScope.ServiceProvider.GetService<UserManager<Person>>();

            // Look for any users
            if (_context.People.Count() != 0)
                return;

            var people = GetPeople().ToArray();
            _context.People.AddRange(people);
            _context.SaveChanges();

            var clients = GetClients(_context).ToArray();
            _context.Clients.AddRange(clients);
            _context.SaveChanges();

            var employees = GetEmployees(_context).ToArray();
            _context.Employees.AddRange(employees);
            _context.SaveChanges();

            CreateRoles(_roleManager);
            AddUsersInRoles(_context, _userManager);

            var addresses = GetAddresses(_context).ToArray();
            _context.Addresses.AddRange(addresses);
            _context.SaveChanges();

            var productCategories = GetProductCategories().ToArray();
            _context.ProductCategories.AddRange(productCategories);
            _context.SaveChanges();

            var products = GetProducts(_context).ToArray();
            _context.Products.AddRange(products);
            _context.SaveChanges();

            GetOrderStates(_context);

            GetOrderPriorities(_context);

            var orderHeaders = GetOrderHeaders().ToArray();
            _context.OrderHeaders.AddRange(orderHeaders);
            _context.SaveChanges();

            var orderLines = GetOrderLines(_context).ToArray();
            _context.OrderLines.AddRange(orderLines);
            _context.SaveChanges();
        }
            //var role = "Admin";
            //var roleStore = new RoleStore<IdentityRole>(_context);
            //roleStore.CreateAsync(new IdentityRole(role));
        //}
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
                new Person{
                    Email = "jp@it.com",
                    UserName = "jp@it.com",
                    NormalizedUserName = "JP@IT.COM",
                    FirstName = "Jake",
                    LastName = "Petrulla"
                },
                new Person{
                    Email = "gf@it.com",
                    UserName = "gf@it.com",
                    NormalizedUserName = "GF@IT.COM",
                    FirstName = "Gerard",
                    LastName = "Ferrer Birbe"
                },
                new Person{
                    Email = "sj@it.com",
                    UserName = "sj@it.com",
                    NormalizedUserName = "SJ@IT.COM",
                    FirstName = "Steve",
                    LastName = "Jobs"
                },
                new Person{
                    Email = "mz@it.com",
                    UserName = "mz@it.com",
                    NormalizedUserName = "MZ@IT.COM",
                    FirstName = "Mark",
                    LastName = "Zuckenberg"
                },
            };
            return people;
        }

        public static List<Client> GetClients(ITAcademyERPContext _context)
        {
            List<Client> clients = new List<Client>()
            {
                new Client{PersonId = _context.People.First(p => p.FirstName == "Mark").Id},
                new Client{PersonId = _context.People.First(p => p.FirstName == "Steve").Id}
            };
            return clients;
        }

        public static List<Employee> GetEmployees(ITAcademyERPContext _context)
        {
            List<Employee> employees = new List<Employee>()
            {
                new Employee{Position = "Master", Salary = 20000, PersonId = _context.People.First(p => p.FirstName == "Jake").Id},
                new Employee{Position = "Student", Salary = 10000, PersonId = _context.People.First(p => p.FirstName == "Gerard").Id}
            };
            return employees;
        }

        public static void CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            RoleManager<IdentityRole> _roleManager = roleManager;
            
            var role1 = new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" };
            _roleManager.CreateAsync(role1).Wait();

            var role2 = new IdentityRole { Name = "Employee", NormalizedName = "EMPLOYEE" };
            _roleManager.CreateAsync(role2).Wait();
        }

        public static void AddUsersInRoles(ITAcademyERPContext _context, UserManager<Person> userManager)
        {
            UserManager<Person> _userManager = userManager;
            
            var user1 = _context.People.First(p => p.FirstName == "Jake");
            _userManager.AddToRolesAsync(user1, new List<string> { "Admin", "Employee"}).Wait();

            var user2 = _context.People.First(p => p.FirstName == "Gerard");
            _userManager.AddToRolesAsync(user2, new List<string> { "Employee" }).Wait();
        }

        public static List<Address> GetAddresses(ITAcademyERPContext _context)
        {           
            List<Address> addresses = new List<Address>()
            {
                new Address{Name = "C/ Montjuic, 127", Type = "Personal", PersonId = _context.People.First(p => p.FirstName == "Jake").Id},
                new Address{Name = "C/ Barcelona, 328", Type = "Delivery",  PersonId = _context.People.First(p => p.FirstName == "Jake").Id},
                new Address{Name = "C/ Perill, 12", Type = "Personal",  PersonId = _context.People.First(p => p.FirstName == "Gerard").Id},
                new Address{Name = "C/ Girona, 124", Type = "Delivery",  PersonId = _context.People.First(p => p.FirstName == "Gerard").Id},
                new Address{Name = "C/ Granollers, 12", Type = "Personal",  PersonId = _context.People.First(p => p.FirstName == "Steve").Id},
                new Address{Name = "C/ Major, 8", Type = "Delivery",  PersonId = _context.People.First(p => p.FirstName == "Steve").Id},
                new Address{Name = "C/ Tort, 12", Type = "Personal",  PersonId = _context.People.First(p => p.FirstName == "Mark").Id},
                new Address{Name = "C/ Llarg, 124", Type = "Delivery",  PersonId = _context.People.First(p => p.FirstName == "Mark").Id}
            };
            return addresses;
        }

        public static List<ProductCategory> GetProductCategories()
        {
            List<ProductCategory> productCategories = new List<ProductCategory>()
            {
                new ProductCategory{ProductCategoryName = "Bicicletes"},
                new ProductCategory{ProductCategoryName = "Motos"},
                new ProductCategory{ProductCategoryName = "Cotxes"},
                new ProductCategory{ProductCategoryName = "Aliments"}
            };
            return productCategories;
        }

        public static List<Product> GetProducts(ITAcademyERPContext _context)
        {
            List<Product> products = new List<Product>()
            {
                new Product{ProductName = "Trek", ProductCategoryId = _context.ProductCategories.First(p => p.ProductCategoryName == "Bicicletes").Id},
                new Product{ProductName = "Montesa", ProductCategoryId = _context.ProductCategories.First(p => p.ProductCategoryName == "Motos").Id},
                new Product{ProductName = "Fiat", ProductCategoryId = _context.ProductCategories.First(p => p.ProductCategoryName == "Cotxes").Id},
                new Product{ProductName = "Poma", ProductCategoryId = _context.ProductCategories.First(p => p.ProductCategoryName == "Aliments").Id}
            };
            return products;
        }        
        
        public static void GetOrderStates(ITAcademyERPContext _context)
        {
            var state1 = new OrderState { State = "Pendent de tractar" };
            _context.OrderStates.Add(state1);           

            var state2 = new OrderState { State = "En tractament" };
            _context.OrderStates.Add(state2);

            var state3 = new OrderState { State = "En repartiment" };
            _context.OrderStates.Add(state3);

            var state4 = new OrderState { State = "Completat" };
            _context.OrderStates.Add(state4);

            var state5 = new OrderState { State = "Cancel·lat" };
            _context.OrderStates.Add(state5);

            _context.SaveChanges();
        }

        public static void GetOrderPriorities(ITAcademyERPContext _context)
        {

            var priority1 = new OrderPriority { Priority = "Baixa" };
            _context.OrderPriorities.Add(priority1);

            var priority2 = new OrderPriority { Priority = "Mitjana" };
            _context.OrderPriorities.Add(priority2);

            var priority3 = new OrderPriority { Priority = "Alta" };
            _context.OrderPriorities.Add(priority3);

            _context.SaveChanges();
        }

        public static List<OrderHeader> GetOrderHeaders()
        {
            List<OrderHeader> orderHeaders = new List<OrderHeader>()
            {
                new OrderHeader
                {
                    OrderNumber = "2020-01",
                    EmployeeId = 1,
                    ClientId = 1,
                    OrderPriorityId = 1,
                    OrderStateId = 5,
                    CreationDate = new DateTime(2020,07,09),
                    AssignToEmployeeDate = new DateTime(2020,08,09),
                    FinalisationDate = new DateTime(2020,09,09)
                },
                new OrderHeader
                {
                    OrderNumber = "2020-02",
                    EmployeeId = 2,
                    ClientId = 2,
                    OrderPriorityId = 3,
                    OrderStateId = 2,
                    CreationDate = new DateTime(2020,08,09),
                    AssignToEmployeeDate = new DateTime(2020,09,09)
                },
                new OrderHeader
                {
                    OrderNumber = "2020-03",
                    EmployeeId = 2,
                    ClientId = 1,
                    OrderPriorityId = 2,
                    OrderStateId = 1,
                    CreationDate = new DateTime(2020,08,11),
                    AssignToEmployeeDate = new DateTime(2020,08,20)
                }
            };
            return orderHeaders;
        }

        public static List<OrderLine> GetOrderLines(ITAcademyERPContext _context)
        {
            List<OrderLine> orderLines = new List<OrderLine>()
            {
                new OrderLine{ProductId = 1, UnitPrice = 100, Vat = 0.21, Quantity = 3, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-01").Id},
                new OrderLine{ProductId = 2, UnitPrice = 200, Vat = 0.21, Quantity = 5, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-01").Id},
                new OrderLine{ProductId = 3, UnitPrice = 300, Vat = 0.21, Quantity = 8, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-01").Id}
            };
            return orderLines;
        }

    }        
}
