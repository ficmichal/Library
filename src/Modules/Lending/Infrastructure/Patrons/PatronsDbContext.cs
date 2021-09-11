using Microsoft.EntityFrameworkCore;

namespace Library.Modules.Lending.Infrastructure.Patrons
{
    public class PatronsDbContext : DbContext
    {
        private readonly string _connectionString;

        public PatronsDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public PatronsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<PatronDatabaseEntity> Patrons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatronDatabaseEntity>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<PatronDatabaseEntity>()
                .Property(x => x.PatronId);

            modelBuilder.Entity<PatronDatabaseEntity>()
                .Property(x => x.PatronType);

            var patronEntityConfig = modelBuilder.Entity<PatronDatabaseEntity>()
                .OwnsMany(x => x.BooksOnHold, books =>
                {
                    books.HasKey(x => x.Id);

                    books.Property(x => x.BookId);

                    books.Property(x => x.LibraryBranchId);

                    books.Property(x => x.Till);

                    books.WithOwner().HasForeignKey(x => x.PatronDatabaseEntity);

                    books.ToTable($"{nameof(HoldDatabaseEntity)}");
                });

            patronEntityConfig.ToTable($"{nameof(PatronDatabaseEntity)}");
        }
    }
}
