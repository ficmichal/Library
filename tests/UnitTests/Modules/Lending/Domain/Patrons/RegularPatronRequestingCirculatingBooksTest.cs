using FluentAssertions;

using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using Library.Modules.Lending.Domain.Patrons.Hold;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Books;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Patrons;

using Xunit;

namespace Library.Modules.Lending.Domain.UnitTests.Patrons
{
    public class RegularPatronRequestingCirculatingBooksTest
    {
        [Theory]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(365)]
        public void a_regular_patron_cannot_place_on_hold_more_than_5_books(int holds)
        {
            // Given
            var aBook = BookFixture.CirculatingBook();
            var regularPatron = PatronFixture.RegularPatronWithHolds(holds);

            // When
            var hold = regularPatron.PlaceOnHold(aBook, HoldDuration.CloseEnded(3));

            // Then
            hold.Should().BeOfType<BookHoldFailed>();

            var bookHoldFailed = hold as BookHoldFailed;

            bookHoldFailed!.Reason.Should().Contain("Patron cannot hold more books");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void a_regular_patron_can_place_on_hold_book_when_he_did_not_place_on_hold_more_than_4_books(int holds)
        {
            // Given
            var aBook = BookFixture.CirculatingBook();
            var regularPatron = PatronFixture.RegularPatronWithHolds(holds);

            // When
            var hold = regularPatron.PlaceOnHold(aBook, HoldDuration.CloseEnded(3));

            // Then
            hold.Should().BeOfType<BookPlacedOnHoldEvents>();
        }
    }
}
