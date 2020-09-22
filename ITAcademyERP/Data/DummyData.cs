using ITAcademyERP.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data
{
    public class DummyData
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<ITAcademyERPContext>();
            context.Database.EnsureCreated();

            // Look for any users
            if (context.ProductCategory.Count() != 0)
                return;

            var addresses = GetAddresses().ToArray();
            context.Address.AddRange(addresses);
            context.SaveChanges();

            var productCategories = GetProductCategories().ToArray();
            context.ProductCategory.AddRange(productCategories);
            context.SaveChanges();
            
            var products = GetProducts().ToArray();
            context.Product.AddRange(products);
            context.SaveChanges();

            var people = GetPeople().ToArray();
            context.Person.AddRange(people);
            context.SaveChanges();

            var clients = GetClients().ToArray();
            context.Client.AddRange(clients);
            context.SaveChanges();

            var employees = GetEmployees().ToArray();
            context.Employee.AddRange(employees);
            context.SaveChanges();

            var orderStates = GetOrderStates().ToArray();
            context.OrderState.AddRange(orderStates);
            context.SaveChanges();

            var orderPriorities = GetOrderPriorities().ToArray();
            context.OrderPriority.AddRange(orderPriorities);
            context.SaveChanges();

            var orderHeaders = GetOrderHeaders().ToArray();
            context.OrderHeader.AddRange(orderHeaders);
            context.SaveChanges();

            var orderLines =GetOrderLines().ToArray();
            context.OrderLine.AddRange(orderLines);
            context.SaveChanges();

        }

        public static List<Address> GetAddresses()
        {
            List<Address> addresses = new List<Address>()
            {
                new Address{AddressName = "C/ Montjuic, 127"},
                new Address{AddressName = "C/ Barcelona, 328"},
                new Address{AddressName = "C/ Perill, 12"}
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

        public static List<Client> GetClients()
        {
            List<Client> clients = new List<Client>()
            {
                new Client{PersonId = 1},
                new Client{PersonId = 2}
            };
            return clients;
        }
        public static List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>()
            {
                new Employee{PersonId = 3, Position = "Director", Salary = 20000},
                new Employee{PersonId = 4, Position = "Manager", Salary = 15000}
            };
            return employees;
        }
        public static List<OrderState> GetOrderStates()
        {
            List<OrderState> orderStates = new List<OrderState>()
            {
                new OrderState{State = "Pendent de tractar"},
                new OrderState{State = "En tractament"},
                new OrderState{State = "En repartiment"}
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
                    ClientId = 1,
                    EmployeeId = 1,
                    OrderPriorityId = 1,
                    OrderStateId = 3,
                    DeliveryAddressId = 1,
                    CreationDate = new DateTime(2020,09,09),
                    AssignToEmployeeDate = new DateTime(2020,09,09),
                    FinalisationDate = new DateTime(2020,09,09)
                },
                new OrderHeader
                {
                    OrderNumber = "2020-02",
                    ClientId = 2,
                    EmployeeId = 1,
                    OrderPriorityId = 3,
                    OrderStateId = 2,
                    DeliveryAddressId = 3,
                    CreationDate = new DateTime(2020,09,09),
                    AssignToEmployeeDate = new DateTime(2020,09,09),
                    FinalisationDate = new DateTime(2020,09,09)                    
                },
                new OrderHeader
                {
                    OrderNumber = "2020-03",
                    ClientId = 1,
                    EmployeeId = 2,
                    OrderPriorityId = 2,
                    OrderStateId = 1,
                    DeliveryAddressId = 2,
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
