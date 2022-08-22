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
            // Given
            var aBook = BookFixture.CirculatingBook();

            // When
            var hold = patron.PlaceOnHold(aBook, HoldDuration.CloseEnded(_min, NumberOfDays.Of(3)));

            // Then
            hold.Should().BeOfType<BookPlacedOnHoldEvents>();

            var bookPlacedOnHoldEvents = hold as BookPlacedOnHoldEvents;
            bookPlacedOnHoldEvents!.MaximumNumberOhHoldsReached.Should().BeNull();

            var bookPlacedOnHold = bookPlacedOnHoldEvents.BookPlacedOnHold;
            bookPlacedOnHold.LibraryBranchId.Should().Be(aBook.LibraryBranchId.Id);
            bookPlacedOnHold.BookId.Should().Be(aBook.Id.Id);
            bookPlacedOnHold.HoldFrom.Should().Be(_min);
            bookPlacedOnHold.HoldTill.Should().Be(_min.AddDays(3));
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(-1)]
        [InlineData(0)]
        public void patron_cannot_hold_a_book_for_0_or_negative_amount_of_days(int days)
        {
            // Given
            var aBook = BookFixture.CirculatingBook();
            var patron = PatronFixture.RegularPatron();

            // When
            Func<IPatronEvent> placingOnHold = () => patron.PlaceOnHold(aBook, HoldDuration.CloseEnded(_min, NumberOfDays.Of(days)));

            // Then
            placingOnHold.Should().Throw<ArgumentException>();
        }


        public static IEnumerable<object[]> AnyPatrons =>
            new List<object[]>
            {
                new object[] { PatronFixture.RegularPatronWithPolicy(new OnlyResearcherPatronsCanPlaceOpenEndedHoldsPolicy()) },
                new object[] { PatronFixture.ResearcherPatronWithPolicy(new OnlyResearcherPatronsCanPlaceOpenEndedHoldsPolicy()) },
            };
    }
}
