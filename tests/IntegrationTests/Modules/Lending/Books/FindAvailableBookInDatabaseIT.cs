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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Library.Modules.Lending.IntegrationTests.Books
{
    public class FindAvailableBookInDatabaseIT
    {
        private static readonly BookId BookId = BookFixture.AnyBookId;
        private static readonly LibraryBranchId LibraryBranchId = LibraryBranchFixture.AnyBranchId;
        private static readonly PatronId PatronId = PatronFixture.AnyPatronId;
        private const string ConnectionString = DbFixture.ConnectionString;

        private readonly BookDatabaseRepository _repo = new(new SqlConnectionFactory(ConnectionString));

        [Fact]
        public async Task should_find_available_book_in_database()
        {
            // Given
            var availableBook = BookFixture.CirculatingAvailableBookAt(BookId, LibraryBranchId);

            // When
            await _repo.Save(availableBook);

            // Then
            (await _repo.FindBy(BookId) as AvailableBook).Should().NotBeNull();

            // When
            var bookOnHold = availableBook.Handle(PlacedOnHold());
            await _repo.Save(bookOnHold);

            // Then
            (await _repo.FindBy(BookId) as AvailableBook).Should().BeNull();
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
