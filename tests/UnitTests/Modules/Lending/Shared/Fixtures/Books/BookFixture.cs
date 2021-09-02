using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Books.Types;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Domain.Patrons;
using System;
using Version = Library.BuildingBlocks.Domain.Version;

namespace Library.Modules.Lending.UnitTests.Shared.Fixtures.Books
{
    public class BookFixture
    {
        public static Version Version0 => Version.Zero();

        public static BookId AnyBookId => new(Guid.NewGuid());

        public static LibraryBranchId AnyBranchId => new(Guid.NewGuid());

        public static PatronId AnyPatronId => new(Guid.NewGuid());

        public static AvailableBook CirculatingBook()
        {
            return new(AnyBookId, BookType.Circulating, Version0, AnyBranchId);
        }
    }
}
