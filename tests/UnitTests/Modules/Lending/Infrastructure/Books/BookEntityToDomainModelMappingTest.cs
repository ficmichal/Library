using FluentAssertions;
using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Books.Types;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Infrastructure.Books;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Books;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.LibraryBranches;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Patrons;
using System;
using Xunit;

namespace Library.Modules.Lending.Infrastructure.UnitTests.Books
{
    public class BookEntityToDomainModelMappingTest
    {
        private static readonly LibraryBranchId LibraryBranchId = LibraryBranchFixture.AnyBranchId;
        private static readonly LibraryBranchId AnotherBranchId = LibraryBranchFixture.AnyBranchId;
        private static readonly LibraryBranchId YetAnotherBranchId = LibraryBranchFixture.AnyBranchId;
        private static readonly PatronId PatronId = PatronFixture.AnyPatronId;
        private static readonly PatronId AnotherPatronId = PatronFixture.AnyPatronId;
        private static readonly BookId BookId = BookFixture.AnyBookId;
        private static readonly DateTime HoldTill = DateTime.Now;

        [Fact]
        public void should_map_to_available_book()
        {
            // Given
            var entity = BookEntity(BookState.Available);

            // When
            var book = entity.ToDomainModel();
            var availableBook = book as AvailableBook;

            // Then
            availableBook!.Id.Should().Be(BookId);
            availableBook.Type.Should().Be(BookType.Circulating);
            availableBook.LibraryBranchId.Should().Be(LibraryBranchId);
        }

        [Fact]
        public void should_map_to_on_hold_book()
        {
            // Given
            var entity = BookEntity(BookState.OnHold);

            // When
            var book = entity.ToDomainModel();
            var bookOnHold = book as BookOnHold;

            // Then
            bookOnHold!.Id.Should().Be(BookId);
            bookOnHold.Type.Should().Be(BookType.Circulating);
            bookOnHold.HoldPlacedAt.Should().Be(AnotherBranchId);
            bookOnHold.ByPatron.Should().Be(PatronId);
            bookOnHold.HoldTill.Should().Be(HoldTill);
        }

        private BookDatabaseEntity BookEntity(BookState state)
        {
            return new()
            {
                BookId = BookId.Id,
                BookType = BookType.Circulating,
                BookState = state,
                AvailableAtBranch = LibraryBranchId.Id,
                OnHoldAtBranch = AnotherBranchId.Id,
                OnHoldByPatron = PatronId.Id,
                OnHoldTill = HoldTill
            };
        }
    }
}
