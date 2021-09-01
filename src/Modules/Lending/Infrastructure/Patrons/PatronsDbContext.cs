using Microsoft.EntityFrameworkCore;

namespace Library.Modules.Lending.Infrastructure.Patrons
{
    public class PatronsDbContext : DbContext
    {
        public DbSet<PatronDatabaseEntity> Patrons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatronDatabaseEntity>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<PatronDatabaseEntity>()
                .Property(x => x.PatronId);

            modelBuilder.Entity<PatronDatabaseEntity>()
                .Property(x => x.PatronType);

            modelBuilder.Entity<PatronDatabaseEntity>()
                .OwnsMany(x => x.BooksOnHold, books =>
                {
                    books.HasKey(x => x.Id);

                    books.Property(x => x.BookId);

                    books.Property(x => x.LibraryBranchId);

                    books.Property(x => x.Till);

                    books.WithOwner().HasForeignKey(x => x.PatronId);
                });
        }
    }
}
