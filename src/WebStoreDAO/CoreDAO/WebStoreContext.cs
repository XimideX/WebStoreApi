using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebStoreModel.Entities;

namespace WebStoreDAO.CoreDAO
{
    public class WebStoreContext : IdentityDbContext
    {
        public WebStoreContext(DbContextOptions<WebStoreContext> options)
            : base(options)
        {
        }
        public WebStoreContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    // .UseLoggerFactory(ConsoleLoggerFactory)
                    .UseMySQL("server=localhost;database=webstore;user=root;password=Vamosleia#-3c",
                        builder => 
                        {
                            builder.MigrationsAssembly("WebStoreService");
                        });
            }
        }

        private static string defaultHistoryMigrationsTableName = "WEBSTORE_HISTORICO_MIGRATIONS";

        public DbSet<Product> Products { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Password).IsRequired();
            });
        }
    }
}