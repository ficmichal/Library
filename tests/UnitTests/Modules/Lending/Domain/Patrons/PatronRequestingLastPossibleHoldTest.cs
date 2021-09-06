using FluentAssertions;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using Library.Modules.Lending.Domain.Patrons.Hold;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Books;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Patrons;
using Xunit;

namespace Library.Modules.Lending.Domain.UnitTests.Patrons
{
    public class PatronRequestingLastPossibleHoldTest
    {
        [Fact]
        public void should_announce_that_a_regular_patron_places_his_last_possible_hold()
        {
            var book = BookFixture.CirculatingBook();

            var hold = PatronFixture.RegularPatronWithHolds(4).PlaceOnHold(book, HoldDuration.CloseEnded(3));

            hold.Should().BeOfType<BookPlacedOnHoldEvents>();

            var @event = hold as BookPlacedOnHoldEvents;
            @event!.MaximumNumberOhHoldsReached.Should().NotBeNull();
            @event.MaximumNumberOhHoldsReached.NumberOfHolds.Should().Be(5);
        }
    }
}
