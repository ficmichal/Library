using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Domain.Patrons;
using System.Collections.Generic;
using System.Linq;

namespace Library.Modules.Lending.Infrastructure.Patrons
{
    public class DomainModelMapper
    {
        public static Patron Map(PatronDatabaseEntity entity)
        {
            return PatronFactory.Create(entity.PatronType, new PatronId(entity.PatronId), MapPatronHolds(entity));
        }

        private static ISet<(BookId, LibraryBranchId)> MapPatronHolds(PatronDatabaseEntity entity)
        {
            return entity.BooksOnHold
                .Select(patron => (new BookId(patron.BookId), new LibraryBranchId(patron.LibraryBranchId)))
                .ToHashSet();
        }
    }
}
