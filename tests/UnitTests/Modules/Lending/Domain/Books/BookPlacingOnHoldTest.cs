using FluentAssertions;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Books;
using System;
using Xunit;

namespace Library.Modules.Lending.Domain.UnitTests.Books
{
    public class BookPlacingOnHoldTest
    {
        private static readonly DateTime Now = DateTime.MinValue;
        private static readonly DateTime OneDayLater = Now.AddDays(1);

        [Fact]
        public void ShouldPlaceOnHoldBookWhichIsMarkedAsAvailableInTheSystem()
        {
            // Given
            var availableBook = BookUnderTest
                .ACirculationBook()
                .With(BookFixture.AnyBookId)
                .LocatedIn(BookFixture.AnyBranchId)
                .StillAvailable();

            var aPatron = BookFixture.AnyPatronId;
            var aBranch = BookFixture.AnyBranchId;

            var bookPlacedOnHoldEvent = PlaceOnHold
                .The(availableBook.BookProvider)
                .By(aPatron)
                .At(aBranch)
                .From(Now)
                .Till(OneDayLater)
                .Place();

            // When
            var bookOnHold = availableBook.ReactsTo(bookPlacedOnHoldEvent);

            // Then
            bookOnHold.Id.Should().Be(availableBook.BookId);
            bookOnHold.ByPatron.Should().Be(aPatron);
            bookOnHold.HoldTill.Should().Be(OneDayLater);
            bookOnHold.HoldPlacedAt.Should().Be(aBranch);
            bookOnHold.Version.Should().Be(availableBook.Version);
        }
    }
}
