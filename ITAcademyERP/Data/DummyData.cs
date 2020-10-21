﻿using ITAcademyERP.Models;
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
            };
            return people;
        }

        public static List<Client> CreateClients(ITAcademyERPContext _context)
        {
            List<Client> clients = new List<Client>()
            {
                new Client{PersonId = _context.People.First(p => p.FirstName == "Mark").Id},
                new Client{PersonId = _context.People.First(p => p.FirstName == "Steve").Id}
            };
            return clients;
        }

        public static List<Employee> CreateEmployees(ITAcademyERPContext _context)
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

        public static List<Address> CreateAddresses(ITAcademyERPContext _context)
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

        public static List<ProductCategory> CreateProductCategories()
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

        public static List<Product> CreateProducts(ITAcademyERPContext _context)
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
        
        public static List<OrderHeader> CreateOrderHeaders(ITAcademyERPContext _context)
        {
            List<OrderHeader> orderHeaders = new List<OrderHeader>()
            {
                new OrderHeader
                {
                    OrderNumber = "2020-03",
                    EmployeeId = _context.Employees.First(o => o.Person.FirstName == "Jake").Id,
                    ClientId = _context.Clients.First(o => o.Person.FirstName == "Steve").Id,
                    OrderPriority = OrderPriority.Baixa,
                    OrderState = OrderState.Completada,
                    CreationDate = new DateTime(2020,07,09),
                    AssignToEmployeeDate = new DateTime(2020,08,09),
                    FinalisationDate = new DateTime(2020,09,09)
                },
                new OrderHeader
                {
                    OrderNumber = "2020-02",
                    EmployeeId = _context.Employees.First(o => o.Person.FirstName == "Jake").Id,
                    ClientId = _context.Clients.First(o => o.Person.FirstName == "Mark").Id,
                    OrderPriority = OrderPriority.Alta,
                    OrderState = OrderState.PendentTractar,
                    CreationDate = new DateTime(2020,08,09),
                    AssignToEmployeeDate = new DateTime(2020,09,09)
                },
                new OrderHeader
                {
                    OrderNumber = "2020-01",
                    EmployeeId = _context.Employees.First(o => o.Person.FirstName == "Gerard").Id,
                    ClientId = _context.Clients.First(o => o.Person.FirstName == "Mark").Id,
                    OrderPriority = OrderPriority.Mitjana,
                    OrderState = OrderState.EnTractament,
                    CreationDate = new DateTime(2020,08,11),
                    AssignToEmployeeDate = new DateTime(2020,08,20)
                }
            };
            return orderHeaders;
        }

        public static List<OrderLine> CreateOrderLines(ITAcademyERPContext _context)
        {
            List<OrderLine> orderLines = new List<OrderLine>()
            {
                new OrderLine{ProductId = _context.Products.First(p => p.ProductName == "Trek").Id, UnitPrice = 100, Vat = 0.21, Quantity = 3, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-01").Id},
                new OrderLine{ProductId = _context.Products.First(p => p.ProductName == "Montesa").Id, UnitPrice = 200, Vat = 0.21, Quantity = 5, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-01").Id},
                new OrderLine{ProductId = _context.Products.First(p => p.ProductName == "Fiat").Id, UnitPrice = 300, Vat = 0.21, Quantity = 8, OrderHeaderId = _context.OrderHeaders.First(o => o.OrderNumber == "2020-02").Id}
            };
            return orderLines;
        }

    }        
}
