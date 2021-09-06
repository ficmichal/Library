using FluentAssertions;
using Library.BuildingBlocks.Infrastructure.Data;
using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Books.Types;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using Library.Modules.Lending.Domain.Patrons.Hold;
using Library.Modules.Lending.Infrastructure.Books;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Books;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Data;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.LibraryBranches;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Patrons;
using System.Threading.Tasks;
using Xunit;

namespace Library.Modules.Lending.IntegrationTests.Books
{
    public class FindBookOnHoldInDatabaseIT
    {
        private static readonly BookId BookId = BookFixture.AnyBookId;
        private static readonly LibraryBranchId LibraryBranchId = LibraryBranchFixture.AnyBranchId;
        private static readonly PatronId PatronId = PatronFixture.AnyPatronId;
        private const string ConnectionString = DbFixture.ConnectionString;

        private readonly BookDatabaseRepository _repo = new(new SqlConnectionFactory(ConnectionString));

        [Fact]
        public async Task should_find_book_on_hold_in_database()
        {
            // Given
            var availableBook = BookFixture.CirculatingAvailableBookAt(BookId, LibraryBranchId);

            // When
            await _repo.Save(availableBook);

            // Then
            (await _repo.FindBy(BookId) as BookOnHold).Should().BeNull();

            // When
            var bookOnHold = availableBook.Handle(PlacedOnHold());
            await _repo.Save(bookOnHold);

            // Then
            (await _repo.FindBy(BookId) as BookOnHold).Should().NotBeNull();
        }

        private BookPlacedOnHold PlacedOnHold()
        {
            return
                BookPlacedOnHold.BookPlacedOnHoldNow(PatronId,
                    BookId, BookType.Circulating,
                    LibraryBranchId,
                    HoldDuration.CloseEnded(5));
        }
    }
}
