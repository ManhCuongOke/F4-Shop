using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Project.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Order> Orders { get; set; }

        public IEnumerable<Product> DisplayProducts (int categoryID) {
            SqlParameter categoryIDParam = new SqlParameter("@FK_iCategoryID", categoryID);
            return this.Products.FromSqlRaw("EXEC sp_SelectProductsByCategoryID @FK_iCategoryID", categoryIDParam);
        }

        public IEnumerable<Product> DisplayProductsPagination (int pageSize, int pageNumber) {
            SqlParameter pageSizeParam = new SqlParameter("@PageSize", pageSize);
            SqlParameter pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            return this.Products.FromSqlRaw("EXEC sp_PaginationProducts @PageSize, @PageNumber", pageSizeParam, pageNumberParam);
        }

        public IEnumerable<Category> DisplayCategories () {
            return this.Categories.FromSqlRaw("select * from tbl_Categories");
        }

        public IEnumerable<Product> DisplayProductByID(int id) {
            SqlParameter productIDParam = new SqlParameter("@PK_iProductID", id);
            return Products.FromSqlRaw("EXEC sp_SelectProductByID @PK_iProductID", productIDParam);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => {
                entity.HasKey(e => e.PK_iUserID);
            });

            modelBuilder.Entity<Category>(entity => {
                entity.HasKey(e => e.PK_iCategoryID);
            });

            modelBuilder.Entity<Product>(entity => {
                entity.HasKey(e => e.PK_iProductID);
            });

            modelBuilder.Entity<User>(entity => {
                entity.HasKey(e => e.PK_iUserID);
            });

            modelBuilder.Entity<Cart>(entity => {
                entity.HasNoKey();
            });

            modelBuilder.Entity<Order>(entity => {
                entity.HasNoKey();
            });

            modelBuilder.Entity<CartDetail>(entity => {
                entity.HasNoKey();
            });
        }
    }
}
