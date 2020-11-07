using ITAcademyERP.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace ITAcademyERP.Data
{
    public partial class ITAcademyERPContext : IdentityDbContext<Person>
    {
        public ITAcademyERPContext()
        {
        }

        public ITAcademyERPContext(DbContextOptions<ITAcademyERPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<OrderHeader> OrderHeaders { get; set; }
        public virtual DbSet<OrderLine> OrderLines { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Address>().ToTable("Addresses");
            builder.Entity<Address>().HasKey(p => p.Id);
            builder.Entity<Address>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Address>().Property(p => p.Name).IsRequired().HasMaxLength(30);
            builder.Entity<Address>().Property(p => p.Type).IsRequired();

            builder.Entity<Person>().ToTable("AspNetUsers");
            builder.Entity<Person>().HasKey(p => p.Id);
            builder.Entity<Person>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Person>().Property(p => p.FirstName).IsRequired().HasMaxLength(30);
            builder.Entity<Person>().Property(p => p.LastName).IsRequired().HasMaxLength(40);
            builder.Entity<Person>().Property(p => p.Email).IsRequired().HasMaxLength(30);
            builder.Entity<Person>().HasMany(p => p.Addresses).WithOne(p => p.Person).HasForeignKey(p => p.PersonId);
            builder.Entity<Person>().HasOne(p => p.Client).WithOne(p => p.Person).HasForeignKey<Client>(p => p.PersonId);
            builder.Entity<Person>().HasOne(p => p.Employee).WithOne(p => p.Person).HasForeignKey<Employee>(p => p.PersonId);

            builder.Entity<Client>().ToTable("Clients");
            builder.Entity<Client>().HasKey(p => p.Id);
            builder.Entity<Client>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Client>().HasMany(p => p.OrderHeaders).WithOne(p => p.Client).HasForeignKey(p => p.ClientId);

            builder.Entity<Employee>().ToTable("Employees");
            builder.Entity<Employee>().HasKey(p => p.Id);
            builder.Entity<Employee>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Employee>().Property(p => p.Position).IsRequired().HasMaxLength(30);
            builder.Entity<Employee>().HasMany(p => p.OrderHeaders).WithOne(p => p.Employee).HasForeignKey(p => p.EmployeeId);

            builder.Entity<ProductCategory>().ToTable("ProductCategories");
            builder.Entity<ProductCategory>().HasKey(p => p.Id);
            builder.Entity<ProductCategory>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<ProductCategory>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<ProductCategory>().HasMany(p => p.Products).WithOne(p => p.Category).HasForeignKey(p => p.CategoryId);

            builder.Entity<Product>().ToTable("Products");
            builder.Entity<Product>().HasKey(p => p.Id);
            builder.Entity<Product>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Product>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<Product>().HasMany(p => p.OrderLines).WithOne(p => p.Product).HasForeignKey(p => p.ProductId);

            builder.Entity<OrderHeader>().ToTable("OrderHeaders");
            builder.Entity<OrderHeader>().HasKey(p => p.Id);
            builder.Entity<OrderHeader>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<OrderHeader>().Property(p => p.OrderNumber).IsRequired().HasMaxLength(20);
            builder.Entity<OrderHeader>().Property(p => p.OrderState).IsRequired();
            builder.Entity<OrderHeader>().Property(p => p.OrderPriority).IsRequired();
            builder.Entity<OrderHeader>().Property(p => p.CreationDate).IsRequired();
            builder.Entity<OrderHeader>().Property(p => p.AssignToEmployeeDate).IsRequired();
            builder.Entity<OrderHeader>().HasMany(p => p.OrderLines).WithOne(p => p.OrderHeader).HasForeignKey(p => p.OrderHeaderId);

            builder.Entity<OrderLine>().ToTable("OrderLines");
            builder.Entity<OrderLine>().HasKey(p => p.Id);
            builder.Entity<OrderLine>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<OrderLine>().Property(p => p.UnitPrice).IsRequired();
            builder.Entity<OrderLine>().Property(p => p.Vat).IsRequired();
            builder.Entity<OrderLine>().Property(p => p.Quantity).IsRequired();


            builder.Entity<ProductCategory>()
                .HasIndex(p => p.Name)
                .IsUnique();

            builder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique();

            builder.Entity<Person>()
                .HasIndex(p => p.Email)
                .IsUnique();
        }

    }
}
