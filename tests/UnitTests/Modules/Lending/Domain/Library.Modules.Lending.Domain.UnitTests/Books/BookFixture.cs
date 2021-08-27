using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Domain.Patrons;
using System;
using Version = Library.BuildingBlocks.Domain.Version;

namespace Library.Modules.Lending.Domain.UnitTests.Books
{
    public class BookFixture
    {
        public static Version Version0 => Version.Zero();

        public static BookId AnyBookId => new(Guid.NewGuid());

        public static LibraryBranchId AnyBranchId => new(Guid.NewGuid());

        public static PatronId AnyPatronId => new(Guid.NewGuid());
    }
}
