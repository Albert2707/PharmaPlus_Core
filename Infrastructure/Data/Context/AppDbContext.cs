using Domain.Entities;
using Infrastructure.Data.Configurations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Context
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _config;
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        // Constructor por defecto para migraciones
        //public AppDbContext() : base(new DbContextOptionsBuilder<AppDbContext>()
        //                                .UseSqlServer(_config["ConnectionStrings:DefaultConnection"])
        //                                .Options)
        //{
        //}
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Types> Types { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ProductCatalog> ProductCatalogs { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string con = ConfigurationManager.ConnectionStrings["CN"].ConnectionString;
            optionsBuilder.UseSqlServer(
                _config["ConnectionStrings:DefaultConnection"],
                b => b.MigrationsAssembly("Infrastructure")); // Asegúrate de usar el nombre del ensamblado donde quieres las migraciones
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                       .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<ProductCatalog>()
                .HasKey(pc => new { pc.ProductCode, pc.SupplierRNC });

            // Configuración de relaciones
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Employee)
                .WithMany(e => e.Sales)
                .HasForeignKey(s => s.EmployeeCode);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Customer)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.ClientId);

            // Configuración de propiedades
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Invoice>()
                .Property(i => i.Total)
                .HasColumnType("decimal(18,2)");

            // Configuración de índices (opcional)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<Employee>()
    .HasOne(e => e.Position)
    .WithMany(p => p.Employees)
    .HasForeignKey(e => e.PositionId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Type)
                .WithMany(t => t.Products)
                .HasForeignKey(p => p.TypeId);
            modelBuilder.Entity<Customer>()
    .Property(c => c.IdentificationNumber)
    .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Employee>()
    .HasKey(e => e.EmployeeCode);

            modelBuilder.Entity<Invoice>()
.HasKey(e => e.InvoiceNumber);

            modelBuilder.Entity<Product>()
.HasKey(e => e.ProductCode);

            modelBuilder.Entity<Supplier>()
                .HasKey(e => e.RNC);

            modelBuilder.Entity<Types>()
    .HasKey(e => e.TypeId);

            base.OnModelCreating(modelBuilder);
        }

        public async Task<List<ProductFilterResult>> FilterProductsByCategoryAsync(int categoryId)
        {
            // Creamos el parametro sql
            var categoryParam = new SqlParameter("@CategoryId", categoryId);
            //var parameter = new ObjectParameter();
            var result = await this.Database.SqlQueryRaw<ProductFilterResult>(
    "EXEC FilterProductsByCategory @CategoryId", categoryParam
).ToListAsync();
            var res = result;
            return res;
        }
    }
}
