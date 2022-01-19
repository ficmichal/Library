using FluentAssertions;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using Library.Modules.Lending.Domain.Patrons.Hold;
using Library.Modules.Lending.Domain.Patrons.Policies;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Books;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Patrons;
using System;
using System.Collections.Generic;
using Xunit;

namespace Library.Modules.Lending.Domain.UnitTests.Patrons
{
    public class PatronRequestingCloseEndedHoldTest
    {
        private readonly DateTime _min = DateTime.MinValue;

        [Theory]
        [MemberData(nameof(AnyPatrons))]
        public void any_patron_can_request_close_ended_hold(Patron patron)
        {
            var aBook = BookFixture.CirculatingBook();

            var hold = patron.PlaceOnHold(aBook, HoldDuration.CloseEnded(_min, NumberOfDays.Of(3)));

            hold.Should().BeOfType<BookPlacedOnHoldEvents>();

            var bookPlacedOnHoldEvents = hold as BookPlacedOnHoldEvents;
            bookPlacedOnHoldEvents!.MaximumNumberOhHoldsReached.Should().BeNull();

            var bookPlacedOnHold = bookPlacedOnHoldEvents.BookPlacedOnHold;
            bookPlacedOnHold.LibraryBranchId.Should().Be(aBook.LibraryBranchId.Id);
            bookPlacedOnHold.BookId.Should().Be(aBook.Id.Id);
            bookPlacedOnHold.HoldFrom.Should().Be(_min);
            bookPlacedOnHold.HoldTill.Should().Be(_min.AddDays(3));
        }

        public static IEnumerable<object[]> AnyPatrons =>
            new List<object[]>
            {
                new object[] { PatronFixture.RegularPatronWithPolicy(new OnlyResearcherPatronsCanPlaceOpenEndedHoldsPolicy()) },
                new object[] { PatronFixture.ResearcherPatronWithPolicy(new OnlyResearcherPatronsCanPlaceOpenEndedHoldsPolicy()) },
            };
    }
}
