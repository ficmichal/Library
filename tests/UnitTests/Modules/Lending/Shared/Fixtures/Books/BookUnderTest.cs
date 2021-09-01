using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Books.Types;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using System;
using Version = Library.BuildingBlocks.Domain.Version;

namespace Library.Modules.Lending.UnitTests.Shared.Fixtures.Books
{
    public class BookUnderTest
    {
        public BookType BookType { get; }

        public BookId BookId { get; private set; }

        public LibraryBranchId LibraryBranchId { get; private set; }

        public PatronId PatronId { get; }

        public Func<IBook> BookProvider { get; private set; }

        public Version Version => BookFixture.Version0;

        public static BookUnderTest ACirculationBook() => new(BookType.Circulating);

        private BookUnderTest(BookType bookType)
        {
            BookType = bookType;
        }

        public BookUnderTest With(BookId bookId)
        {
            BookId = bookId;

            return this;
        }

        public BookUnderTest LocatedIn(LibraryBranchId libraryBranchId)
        {
            LibraryBranchId = libraryBranchId;

            return this;
        }

        public BookUnderTest StillAvailable()
        {
            BookProvider = () => new AvailableBook(BookId, BookType, Version, LibraryBranchId);

            return this;
        }

        public BookOnHold ReactsTo(BookPlacedOnHold @event)
        {
            return (BookProvider() as AvailableBook)?.Handle(@event);
        }
    }
}
