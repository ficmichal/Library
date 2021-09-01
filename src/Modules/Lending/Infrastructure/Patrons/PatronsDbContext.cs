using Microsoft.EntityFrameworkCore;

namespace Library.Modules.Lending.Infrastructure.Patrons
{
    public class PatronsDbContext : DbContext
    {
        public DbSet<PatronDatabaseEntity> Patrons { get; set; }
    }
}
