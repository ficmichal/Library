using FluentAssertions;
using Library.BuildingBlocks.Infrastructure.Data;
using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Books.Types;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Infrastructure.Books;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Books;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Data;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.LibraryBranches;
using System.Threading.Tasks;
using Xunit;

namespace Library.Modules.Lending.IntegrationTests.Books
{
    public class BookDatabaseRepositoryIT
    {
        private static readonly BookId BookId = BookFixture.AnyBookId;
        private static readonly LibraryBranchId LibraryBranchId = LibraryBranchFixture.AnyBranchId;
        private const string ConnectionString = DbFixture.ConnectionString;

        private readonly BookDatabaseRepository _repo = new (new SqlConnectionFactory(ConnectionString));

        [Fact]
        public async Task persistence_in_real_database_should_work()
        {
            // Given
            var availableBook = BookFixture.CirculatingAvailableBookAt(BookId, LibraryBranchId);

            // When
            await _repo.Save(availableBook);

            // Then
            await BookIsPersistedAs<AvailableBook>();
        }

        private async Task BookIsPersistedAs<T>()
        {
            var book = await LoadPersistedBook(BookId);

            book.Should().BeOfType<T>();
        }

        private async Task<IBook> LoadPersistedBook(BookId bookId)
        {
            var loaded = await _repo.FindBy(bookId);

            return loaded;
        }
    }
}
