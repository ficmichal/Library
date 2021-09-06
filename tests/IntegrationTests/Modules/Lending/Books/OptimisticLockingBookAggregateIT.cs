using FluentAssertions;
using Library.BuildingBlocks.Domain.Exceptions;
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
    public class OptimisticLockingBookAggregateIT
    {
        private static readonly BookId BookId = BookFixture.AnyBookId;
        private static readonly LibraryBranchId LibraryBranchId = LibraryBranchFixture.AnyBranchId;
        private static readonly PatronId PatronId = PatronFixture.AnyPatronId;
        private const string ConnectionString = DbFixture.ConnectionString;

        private readonly BookDatabaseRepository _repo = new(new SqlConnectionFactory(ConnectionString));

        [Fact]
        public async Task optimistic_locking_in_real_database_should_work()
        {
            // Given
            var availableBook = BookFixture.CirculatingAvailableBookAt(BookId, LibraryBranchId);

            await _repo.Save(availableBook);
            var loaded = await LoadPersistedBook(BookId);
            await SomeoneModifiedBookInTheMeantime(availableBook);

            // When
            Func<Task> savingLoadedBookToRepo = async () => await _repo.Save(loaded);

            // Then
            await savingLoadedBookToRepo.Should().ThrowAsync<AggregateRootIsStale>();
            (await LoadPersistedBook(BookId)).Version.Should().Be(BuildingBlocks.Domain.Version.Of(1));
        }

        private async Task SomeoneModifiedBookInTheMeantime(AvailableBook availableBook)
        {
            await _repo.Save(availableBook.Handle(PlacedOnHold(PatronId)));
        }

        private async Task<IBook> LoadPersistedBook(BookId bookId)
        {
            var loaded = await _repo.FindBy(bookId);

            return loaded;
        }

        private BookPlacedOnHold PlacedOnHold(PatronId patronId)
        {
            return
                BookPlacedOnHold.BookPlacedOnHoldNow(patronId,
                    BookId, BookType.Circulating,
                    LibraryBranchId,
                    HoldDuration.CloseEnded(5));
        }
    }
}
