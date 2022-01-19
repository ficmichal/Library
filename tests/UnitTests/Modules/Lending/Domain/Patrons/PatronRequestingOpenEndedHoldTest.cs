using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using Library.Modules.Lending.Domain.Patrons.Hold;
using Library.Modules.Lending.Domain.Patrons.Policies;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Books;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Patrons;
using Xunit;

namespace Library.Modules.Lending.Domain.UnitTests.Patrons
{
    public class PatronRequestingOpenEndedHoldTest
    {
        private readonly DateTime _from = DateTime.MinValue;

        [Fact]
        public void researcher_patron_can_request_close_ended_hold()
        {
            // Given
            var aBook = BookFixture.CirculatingBook();
            var researcherPatron = PatronFixture.ResearcherPatronWithPolicy(new OnlyResearcherPatronsCanPlaceOpenEndedHoldsPolicy());
            var patronId = researcherPatron.PatronInformation.PatronId;

            // When
            var hold = researcherPatron.PlaceOnHold(aBook, HoldDuration.OpenEnded(_from));

            // Then
            hold.Should().BeOfType<BookPlacedOnHoldEvents>();

            var bookPlacedOnHold = (hold as BookPlacedOnHoldEvents)!.BookPlacedOnHold;
            bookPlacedOnHold.LibraryBranchId.Should().Be(aBook.LibraryBranchId.Id);
            bookPlacedOnHold.PatronIdValue.Should().Be(patronId.Id);
            bookPlacedOnHold.BookId.Should().Be(aBook.Id.Id);
            bookPlacedOnHold.HoldFrom.Should().Be(_from);
            bookPlacedOnHold.HoldTill.Should().Be(null);
        }

        [Fact]
        public void regular_patron_cannot_request_open_ended_hold()
        {
            // Given
            var aBook = BookFixture.CirculatingBook();
            var regularPatron = PatronFixture.RegularPatronWithPolicy(new OnlyResearcherPatronsCanPlaceOpenEndedHoldsPolicy());
            var patronId = regularPatron.PatronInformation.PatronId;

            // When
            var hold = regularPatron.PlaceOnHold(aBook, HoldDuration.OpenEnded(_from));

            // Then
            hold.Should().BeOfType<BookHoldFailed>();

            var bookHoldFailed = hold as BookHoldFailed;

            bookHoldFailed!.LibraryBranchId.Should().Be(aBook.LibraryBranchId.Id);
            bookHoldFailed.PatronIdValue.Should().Be(patronId.Id);
            bookHoldFailed.BookId.Should().Be(aBook.Id.Id);
            bookHoldFailed.Reason.Should().Contain("Regular patron cannot place open ended holds");
        }
    }
}
