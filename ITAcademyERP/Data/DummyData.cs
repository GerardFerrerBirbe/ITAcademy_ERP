using ITAcademyERP.Data.Resources;
using ITAcademyERP.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ITAcademyERP.Data
{
    public class DummyData
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var _context = serviceScope.ServiceProvider.GetService<ITAcademyERPContext>();
            _context.Database.EnsureCreated();
            var _roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
            var _userManager = serviceScope.ServiceProvider.GetService<UserManager<Person>>();

            // Look for any users
            if (_context.People.Count() != 0)
                return;

            var people = CreatePeople().ToArray();
            _context.People.AddRange(people);
            _context.SaveChanges();

            var clients = CreateClients(_context).ToArray();
            _context.Clients.AddRange(clients);
            _context.SaveChanges();

            var employees = CreateEmployees(_context).ToArray();
            _context.Employees.AddRange(employees);
            _context.SaveChanges();

            CreateRoles(_roleManager);
            AddUsersInRoles(_context, _userManager);

            var addresses = CreateAddresses(_context).ToArray();
            _context.Addresses.AddRange(addresses);
            _context.SaveChanges();

            var productCategories = CreateProductCategories().ToArray();
            _context.ProductCategories.AddRange(productCategories);
            _context.SaveChanges();

            var products = CreateProducts(_context).ToArray();
            _context.Products.AddRange(products);
            _context.SaveChanges();

            var orderHeaders = CreateOrderHeaders(_context).ToArray();
            _context.OrderHeaders.AddRange(orderHeaders);
            _context.SaveChanges();

            var orderLines = CreateOrderLines(_context).ToArray();
            _context.OrderLines.AddRange(orderLines);
            _context.SaveChanges();
        }         

        public static List<Person> CreatePeople()
        {
            List<Person> people = new List<Person>()
            {
                new Person{
                    Email = "jp@it.com",
                    NormalizedEmail = "JP@IT.COM",
                    UserName = "jp@it.com",
                    NormalizedUserName = "JP@IT.COM",
                    FirstName = "Jake",
                    LastName = "Petrulla"
                },
                new Person{
                    Email = "gf@it.com",
                    NormalizedEmail = "GF@IT.COM",
                    UserName = "gf@it.com",
                    NormalizedUserName = "GF@IT.COM",
                    FirstName = "Gerard",
                    LastName = "Ferrer Birbe"
                },
                new Person{
                    Email = "sj@it.com",
                    NormalizedEmail = "SJ@IT.COM",
                    UserName = "sj@it.com",
                    NormalizedUserName = "SJ@IT.COM",
                    FirstName = "Steve",
                    LastName = "Jobs"
                },
                new Person{
                    Email = "mz@it.com",
                    NormalizedEmail = "MZ@IT.COM",
                    UserName = "mz@it.com",
                    NormalizedUserName = "MZ@IT.COM",
                    FirstName = "Mark",
                    LastName = "Zuckenberg"
                },
                new Person{
                    Email = "pc@it.com",
                    NormalizedEmail = "PC@IT.COM",
                    UserName = "pc@it.com",
                    NormalizedUserName = "PC@IT.COM",
                    FirstName = "Philip",
                    LastName = "Collins"
                },
                new Person{
                    Email = "gfg@it.com",
                    NormalizedEmail = "GFG@IT.COM",
                    UserName = "gfg@it.com",
                    NormalizedUserName = "GFG@IT.COM",
                    FirstName = "Georgina",
                    LastName = "Fernandez Gomez"
                },
                new Person{
                    Email = "sr@it.com",
                    NormalizedEmail = "SR@IT.COM",
                    UserName = "sr@it.com",
                    NormalizedUserName = "SR@IT.COM",
                    FirstName = "Santi",
                    LastName = "Rossinyol"
                },
                new Person{
                    Email = "jb@it.com",
                    NormalizedEmail = "JB@IT.COM",
                    UserName = "jb@it.com",
                    NormalizedUserName = "JB@IT.COM",
                    FirstName = "Josep",
                    LastName = "Batalla"
                },
                new Person{
                    Email = "jpc@it.com",
                    NormalizedEmail = "JPC@IT.COM",
                    UserName = "jpc@it.com",
                    NormalizedUserName = "JPC@IT.COM",
                    FirstName = "Joan",
                    LastName = "Patrici Carrasco"
                },
                new Person{
                    Email = "gmc@it.com",
                    NormalizedEmail = "GMC@IT.COM",
                    UserName = "gmc@it.com",
                    NormalizedUserName = "GMC@IT.COM",
                    FirstName = "Gregori",
                    LastName = "Martí Carrión"
                },
                new Person{
                    Email = "sv@it.com",
                    NormalizedEmail = "SV@IT.COM",
                    UserName = "sv@it.com",
                    NormalizedUserName = "SV@IT.COM",
                    FirstName = "Silvia",
                    LastName = "Vallvé"
                },
                new Person{
                    Email = "mrc@it.com",
                    NormalizedEmail = "MRC@IT.COM",
                    UserName = "mrc@it.com",
                    NormalizedUserName = "MRC@IT.COM",
                    FirstName = "Maria",
                    LastName = "Rodriguez Català"
                },
            };
            return people;
        }

        public static List<Client> CreateClients(ITAcademyERPContext _context)
        {
            List<Client> clients = new List<Client>()
            {
                new Client{PersonId = _context.People.First(p => p.FirstName == "Mark").Id},
                new Client{PersonId = _context.People.First(p => p.FirstName == "Steve").Id},
                new Client{PersonId = _context.People.First(p => p.FirstName == "Maria").Id},
                new Client{PersonId = _context.People.First(p => p.FirstName == "Silvia").Id},
                new Client{PersonId = _context.People.First(p => p.FirstName == "Gregori").Id},
                new Client{PersonId = _context.People.First(p => p.FirstName == "Philip").Id},
                new Client{PersonId = _context.People.First(p => p.FirstName == "Georgina").Id},
                new Client{PersonId = _context.People.First(p => p.FirstName == "Jake").Id}
            };
            return clients;
        }

        public static List<Employee> CreateEmployees(ITAcademyERPContext _context)
        {
            List<Employee> employees = new List<Employee>()
            {
                new Employee{Position = "Master", Salary = 20000, PersonId = _context.People.First(p => p.FirstName == "Jake").Id},
                new Employee{Position = "Student", Salary = 10000, PersonId = _context.People.First(p => p.FirstName == "Gerard").Id},
                new Employee{Position = "Master", Salary = 20000, PersonId = _context.People.First(p => p.FirstName == "Joan").Id},
                new Employee{Position = "Student", Salary = 10000, PersonId = _context.People.First(p => p.FirstName == "Josep").Id},
                new Employee{Position = "Master", Salary = 20000, PersonId = _context.People.First(p => p.FirstName == "Georgina").Id},
                new Employee{Position = "Student", Salary = 10000, PersonId = _context.People.First(p => p.FirstName == "Santi").Id},
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

        public static List<Address> CreateAddresses(ITAcademyERPContext _context)
        {           
            List<Address> addresses = new List<Address>()
            {
                new Address{Name = "C/ Montjuic, 127", Type = EAddressType.Personal, PersonId = _context.People.First(p => p.FirstName == "Jake").Id},
                new Address{Name = "C/ Barcelona, 328", Type = EAddressType.Entrega,  PersonId = _context.People.First(p => p.FirstName == "Jake").Id},
                new Address{Name = "C/ Perill, 12", Type = EAddressType.Personal,  PersonId = _context.People.First(p => p.FirstName == "Gerard").Id},
                new Address{Name = "C/ Girona, 124", Type = EAddressType.Entrega,  PersonId = _context.People.First(p => p.FirstName == "Gerard").Id},
                new Address{Name = "C/ Granollers, 12", Type = EAddressType.Personal,  PersonId = _context.People.First(p => p.FirstName == "Steve").Id},
                new Address{Name = "C/ Major, 8", Type = EAddressType.Entrega,  PersonId = _context.People.First(p => p.FirstName == "Steve").Id},
                new Address{Name = "C/ Tort, 12", Type = EAddressType.Personal,  PersonId = _context.People.First(p => p.FirstName == "Mark").Id},
                new Address{Name = "C/ Llarg, 200", Type = EAddressType.Entrega,  PersonId = _context.People.First(p => p.FirstName == "Maria").Id},
                new Address{Name = "C/ Joanic, 12", Type = EAddressType.Entrega,  PersonId = _context.People.First(p => p.FirstName == "Silvia").Id},
                new Address{Name = "C/ Ample, 1", Type = EAddressType.Entrega,  PersonId = _context.People.First(p => p.FirstName == "Gregori").Id},
                new Address{Name = "C/ Curt, 320", Type = EAddressType.Entrega,  PersonId = _context.People.First(p => p.FirstName == "Philip").Id},
                new Address{Name = "C/ Mallorca, 32", Type = EAddressType.Entrega,  PersonId = _context.People.First(p => p.FirstName == "Georgina").Id}
            };
            return addresses;
        }

        public static List<ProductCategory> CreateProductCategories()
        {
            List<ProductCategory> productCategories = new List<ProductCategory>()
            {
                new ProductCategory{Name = "Bicicletes"},
                new ProductCategory{Name = "Motos"},
                new ProductCategory{Name = "Cotxes"},
                new ProductCategory{Name = "Aliments"},
                new ProductCategory{Name = "Camions"},
                new ProductCategory{Name = "Avions"},
                new ProductCategory{Name = "Ordinadors"},
                new ProductCategory{Name = "Roba"}
            };
            return productCategories;
        }

        public static List<Product> CreateProducts(ITAcademyERPContext _context)
        {
            List<Product> products = new List<Product>()
            {
                new Product{Name = "Trek", CategoryId = _context.ProductCategories.First(p => p.Name == "Bicicletes").Id},
                new Product{Name = "Montesa", CategoryId = _context.ProductCategories.First(p => p.Name == "Motos").Id},
                new Product{Name = "Fiat", CategoryId = _context.ProductCategories.First(p => p.Name == "Cotxes").Id},
                new Product{Name = "Poma", CategoryId = _context.ProductCategories.First(p => p.Name == "Aliments").Id},
                new Product{Name = "Orbea", CategoryId = _context.ProductCategories.First(p => p.Name == "Bicicletes").Id},
                new Product{Name = "Bultaco", CategoryId = _context.ProductCategories.First(p => p.Name == "Motos").Id},
                new Product{Name = "Ford", CategoryId = _context.ProductCategories.First(p => p.Name == "Cotxes").Id},
                new Product{Name = "Pera", CategoryId = _context.ProductCategories.First(p => p.Name == "Aliments").Id},
                new Product{Name = "Canondale", CategoryId = _context.ProductCategories.First(p => p.Name == "Bicicletes").Id},
                new Product{Name = "Suzuki", CategoryId = _context.ProductCategories.First(p => p.Name == "Motos").Id},
                new Product{Name = "VW", CategoryId = _context.ProductCategories.First(p => p.Name == "Cotxes").Id},
                new Product{Name = "Entrecot", CategoryId = _context.ProductCategories.First(p => p.Name == "Aliments").Id},
                new Product{Name = "Man", CategoryId = _context.ProductCategories.First(p => p.Name == "Camions").Id},
                new Product{Name = "Boeing", CategoryId = _context.ProductCategories.First(p => p.Name == "Avions").Id},
                new Product{Name = "HP", CategoryId = _context.ProductCategories.First(p => p.Name == "Ordinadors").Id},
                new Product{Name = "Pantalons Zara", CategoryId = _context.ProductCategories.First(p => p.Name == "Roba").Id}
            };
            return products;
        }        
        
        public static List<OrderHeader> CreateOrderHeaders(ITAcademyERPContext _context)
        {
            List<OrderHeader> orderHeaders = new List<OrderHeader>()
            {
                new OrderHeader
                {
                    OrderNumber = "2020-09",
                    EmployeeId = _context.Employees.First(o => o.Person.FirstName == "Jake").Id,
                    ClientId = _context.Clients.First(o => o.Person.FirstName == "Gregori").Id,
                    OrderPriority = EOrderPriority.Baixa,
                    OrderState = EOrderState.Completada,
                    CreationDate = new DateTime(2020,11,09),
                    AssignToEmployeeDate = new DateTime(2020,11,09),
                    FinalisationDate = new DateTime(2020,12,09)
                },
                new OrderHeader
                {
                    OrderNumber = "2020-08",
                    EmployeeId = _context.Employees.First(o => o.Person.FirstName == "Santi").Id,
                    ClientId = _context.Clients.First(o => o.Person.FirstName == "Philip").Id,
                    OrderPriority = EOrderPriority.Alta,
                    OrderState = EOrderState.PendentTractar,
                    CreationDate = new DateTime(2020,09,09),
                    AssignToEmployeeDate = new DateTime(2020,10,09)
                },
                new OrderHeader
                {
                    OrderNumber = "2020-07",
                    EmployeeId = _context.Employees.First(o => o.Person.FirstName == "Georgina").Id,
                    ClientId = _context.Clients.First(o => o.Person.FirstName == "Silvia").Id,
                    OrderPriority = EOrderPriority.Mitjana,
                    OrderState = EOrderState.EnTractament,
                    CreationDate = new DateTime(2020,08,11),
                    AssignToEmployeeDate = new DateTime(2020,08,20)
                },
                new OrderHeader
                {
                    OrderNumber = "2020-06",
                    EmployeeId = _context.Employees.First(o => o.Person.FirstName == "Santi").Id,
                    ClientId = _context.Clients.First(o => o.Person.FirstName == "Silvia").Id,
                    OrderPriority = EOrderPriority.Baixa,
                    OrderState = EOrderState.Completada,
                    CreationDate = new DateTime(2020,06,09),
                    AssignToEmployeeDate = new DateTime(2020,06,09),
                    FinalisationDate = new DateTime(2020,06,09)
                },
                new OrderHeader
                {
                    OrderNumber = "2020-05",
                    EmployeeId = _context.Employees.First(o => o.Person.FirstName == "Georgina").Id,
                    ClientId = _context.Clients.First(o => o.Person.FirstName == "Georgina").Id,
                    OrderPriority = EOrderPriority.Alta,
                    OrderState = EOrderState.PendentTractar,
                    CreationDate = new DateTime(2020,05,09),
                    AssignToEmployeeDate = new DateTime(2020,05,09)
                },
                new OrderHeader
                {
                    OrderNumber = "2020-04",
                    EmployeeId = _context.Employees.First(o => o.Person.FirstName == "Gerard").Id,
                    ClientId = _context.Clients.First(o => o.Person.FirstName == "Maria").Id,
                    OrderPriority = EOrderPriority.Mitjana,
                    OrderState = EOrderState.EnTractament,
                    CreationDate = new DateTime(2020,04,11),
                    AssignToEmployeeDate = new DateTime(2020,04,20)
                },
                new OrderHeader
                {
                    OrderNumber = "2020-03",
                    EmployeeId = _context.Employees.First(o => o.Person.FirstName == "Jake").Id,
                    ClientId = _context.Clients.First(o => o.Person.FirstName == "Steve").Id,
                    OrderPriority = EOrderPriority.Baixa,
                    OrderState = EOrderState.Completada,
                    CreationDate = new DateTime(2020,03,09),
                    AssignToEmployeeDate = new DateTime(2020,03,09),
                    FinalisationDate = new DateTime(2020,03,09)
                },
                new OrderHeader
                {
                    OrderNumber = "2020-02",
                    EmployeeId = _context.Employees.First(o => o.Person.FirstName == "Jake").Id,
                    ClientId = _context.Clients.First(o => o.Person.FirstName == "Mark").Id,
                    OrderPriority = EOrderPriority.Alta,
                    OrderState = EOrderState.PendentTractar,
                    CreationDate = new DateTime(2020,02,09),
                    AssignToEmployeeDate = new DateTime(2020,02,09)
                },
                new OrderHeader
                {
                    OrderNumber = "2020-01",
                    EmployeeId = _context.Employees.First(o => o.Person.FirstName == "Gerard").Id,
                    ClientId = _context.Clients.First(o => o.Person.FirstName == "Mark").Id,
                    OrderPriority = EOrderPriority.Mitjana,
                    OrderState = EOrderState.EnTractament,
                    CreationDate = new DateTime(2020,01,11),
                    AssignToEmployeeDate = new DateTime(2020,01,20)
                }
            };
            return orderHeaders;
        }

        public static List<OrderLine> CreateOrderLines(ITAcademyERPContext _context)
        {
            List<OrderLine> orderLines = new List<OrderLine>()
            {
                new OrderLine{ProductId = _context.Products.First(p => p.Name == "Trek").Id, UnitPrice = 2000, Vat = 0.21, Quantity = 2, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-01").Id},
                new OrderLine{ProductId = _context.Products.First(p => p.Name == "Montesa").Id, UnitPrice =6000, Vat = 0.21, Quantity = 1, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-01").Id},
                new OrderLine{ProductId = _context.Products.First(p => p.Name == "Fiat").Id, UnitPrice = 14000, Vat = 0.21, Quantity = 1, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-02").Id},
                new OrderLine{ProductId = _context.Products.First(p => p.Name == "Man").Id, UnitPrice = 46000, Vat = 0.21, Quantity = 1, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-03").Id},
                new OrderLine{ProductId = _context.Products.First(p => p.Name == "Boeing").Id, UnitPrice = 98000, Vat = 0.21, Quantity = 1, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-04").Id},
                new OrderLine{ProductId = _context.Products.First(p => p.Name == "VW").Id, UnitPrice = 28000, Vat = 0.21, Quantity = 1, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-05").Id},
                new OrderLine{ProductId = _context.Products.First(p => p.Name == "Orbea").Id, UnitPrice = 1200, Vat = 0.21, Quantity = 3, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-06").Id},
                new OrderLine{ProductId = _context.Products.First(p => p.Name == "Suzuki").Id, UnitPrice = 8000, Vat = 0.21, Quantity = 2, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-06").Id},
                new OrderLine{ProductId = _context.Products.First(p => p.Name == "Pantalons Zara").Id, UnitPrice = 90, Vat = 0.21, Quantity = 800, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-07").Id},
                new OrderLine{ProductId = _context.Products.First(p => p.Name == "Poma").Id, UnitPrice = 1.4, Vat = 0.21, Quantity = 3000, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-07").Id},
                new OrderLine{ProductId = _context.Products.First(p => p.Name == "Pera").Id, UnitPrice = 1.8, Vat = 0.21, Quantity = 4000, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-08").Id},
                new OrderLine{ProductId = _context.Products.First(p => p.Name == "Entrecot").Id, UnitPrice = 14, Vat = 0.21, Quantity = 2000, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-08").Id},new OrderLine{ProductId = _context.Products.First(p => p.Name == "Trek").Id, UnitPrice = 100, Vat = 0.21, Quantity = 3, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-01").Id},
                new OrderLine{ProductId = _context.Products.First(p => p.Name == "Bultaco").Id, UnitPrice = 7500, Vat = 0.21, Quantity = 4, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-09").Id},
                new OrderLine{ProductId = _context.Products.First(p => p.Name == "Ford").Id, UnitPrice = 23000, Vat = 0.21, Quantity = 2, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-09").Id}
            };
            return orderLines;
        }

    }        
}
