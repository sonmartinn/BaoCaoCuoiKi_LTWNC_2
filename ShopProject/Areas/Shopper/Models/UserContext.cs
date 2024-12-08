namespace ShopProject.Areas.Shopper.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class UserContext : DbContext
    {
        public UserContext()
            : base("name=UserContext")
        {
        }

        // DbSet cho các entity trong cơ sở dữ liệu
        public virtual DbSet<Account> Accounts { get; set; }  // DbSet cho Account
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }

        // Hàm này được gọi khi EF tạo ra mô hình cơ sở dữ liệu từ lớp này
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình cho các bảng
            modelBuilder.Entity<Account>()
                .Property(e => e.Username)
                .IsUnicode(false);  // Không lưu trữ Unicode cho Username

            modelBuilder.Entity<Account>()
                .Property(e => e.Email)
                .IsUnicode(false);  // Không lưu trữ Unicode cho Email

            modelBuilder.Entity<Account>()
                .Property(e => e.Password)
                .IsUnicode(false);  // Không lưu trữ Unicode cho Password



            // Cấu hình cho các bảng khác (ví dụ: Customer, Order...)
            modelBuilder.Entity<Customer>()
                .Property(e => e.cusPhone)
                .IsUnicode(false);  // Không lưu trữ Unicode cho phone

            modelBuilder.Entity<Customer>()
                .Property(e => e.cusEmail)
                .IsUnicode(false);  // Không lưu trữ Unicode cho email

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.orderID)
                .IsUnicode(false);  // Không lưu trữ Unicode cho orderID

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.proID)
                .IsUnicode(false);  // Không lưu trữ Unicode cho product ID

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.ordtsThanhTien)
                .IsUnicode(false);  // Không lưu trữ Unicode cho price

            modelBuilder.Entity<Order>()
                .Property(e => e.orderID)
                .IsUnicode(false);  // Không lưu trữ Unicode cho order ID

            modelBuilder.Entity<Order>()
                .Property(e => e.cusPhone)
                .IsUnicode(false);  // Không lưu trữ Unicode cho customer phone

            modelBuilder.Entity<Order>()
                .Property(e => e.orderDateTime)
                .IsUnicode(false);  // Không lưu trữ Unicode cho datetime của đơn hàng

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);  // Cấu hình mối quan hệ

            modelBuilder.Entity<Producer>()
                .Property(e => e.pdcPhone)
                .IsUnicode(false);  // Không lưu trữ Unicode cho phone của Producer

            modelBuilder.Entity<Producer>()
                .Property(e => e.pdcEmail)
                .IsUnicode(false);  // Không lưu trữ Unicode cho email của Producer

            modelBuilder.Entity<Product>()
                .Property(e => e.proID)
                .IsUnicode(false);  // Không lưu trữ Unicode cho product ID

            modelBuilder.Entity<Product>()
                .Property(e => e.proSize)
                .IsUnicode(false);  // Không lưu trữ Unicode cho size của sản phẩm

            modelBuilder.Entity<Product>()
                .Property(e => e.proPrice)
                .IsUnicode(false);  // Không lưu trữ Unicode cho price của sản phẩm

            modelBuilder.Entity<Product>()
                .Property(e => e.proUpdateDate)
                .IsUnicode(false);  // Không lưu trữ Unicode cho update date của sản phẩm

            modelBuilder.Entity<Product>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);  // Cấu hình mối quan hệ với OrderDetails

            modelBuilder.Entity<Product>()
                .HasOptional(e => e.Rate)
                .WithRequired(e => e.Product);  // Cấu hình mối quan hệ 1-1 với Rate

            modelBuilder.Entity<Rate>()
                .Property(e => e.proID)
                .IsUnicode(false);  // Không lưu trữ Unicode cho product ID trong bảng Rate
        }
    }
}
