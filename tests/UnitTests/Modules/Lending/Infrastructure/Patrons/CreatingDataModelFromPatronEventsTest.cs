using FluentAssertions;
using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using Library.Modules.Lending.Domain.Patrons.Hold;
using Library.Modules.Lending.Infrastructure.Patrons;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Books;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Patrons;
using System;
using System.Linq;
using Xunit;
using static Library.Modules.Lending.Domain.Patrons.Hold.HoldDuration;

namespace Library.Modules.Lending.Infrastructure.UnitTests.Patrons
{
    public class CreatingDataModelFromPatronEventsTest
    {
        private static PatronId PatronId => PatronFixture.AnyPatronId;
        private static PatronType Regular => PatronType.Regular;
        private static LibraryBranchId LibraryBranchId => BookFixture.AnyBranchId;
        private static BookType Type => BookType.Restricted;
        private static BookId BookId => BookFixture.AnyBookId;

        private static readonly DateTime HoldFrom = DateTime.Now;

        [Fact]
        public void ShouldAddHoldOnPlacedOnHoldEventWithCloseEndedDuration()
        {
            // Given
            var entity = CreatePatron();

            // When
            entity.Handle(PlacedOnHold(CloseEnded(HoldFrom, NumberOfDays.Of(1))));

            // Then
            entity.BooksOnHold.Count.Should().Be(1);
            entity.BooksOnHold.First().Till.Should().Be(HoldFrom.AddDays(1));
        }

        private PatronDatabaseEntity CreatePatron()
        {
            return new()
            {
                PatronId = PatronId.Id,
                PatronType = Regular
            };
        }

        private BookPlacedOnHoldEvents PlacedOnHold(HoldDuration duration)
        {
            return BookPlacedOnHoldEvents.Events(
                BookPlacedOnHold.BookPlacedOnHoldNow(
                    PatronId,
                    BookId,
                    Type,
                    LibraryBranchId,
                    duration));
        }
    }
}
