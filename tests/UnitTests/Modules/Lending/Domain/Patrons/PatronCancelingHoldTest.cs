using FluentAssertions;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Books;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Patrons;
using System.Threading.Tasks;
using Xunit;

namespace Library.Modules.Lending.Domain.UnitTests.Patrons
{
    public class PatronCancelingHoldTest
    {
        [Fact]
        public void patron_should_be_able_to_cancel_his_holds()
        {
            // Given
            var bookOnHold = BookFixture.BookOnHold();
            var patron = PatronFixture.RegularPatronWithHold(PatronFixture.AnyPatronId, bookOnHold);

            // When
            var cancelHold = patron.CancelHold(bookOnHold);

            // Then
            cancelHold.Should().BeOfType<BookHoldCanceled>();

            var bookHoldCanceledEvent = cancelHold as BookHoldCanceled;
            bookHoldCanceledEvent!.LibraryBranchId.Should().Be(bookOnHold.HoldPlacedAt.Id);
            bookHoldCanceledEvent.BookId.Should().Be(bookOnHold.Id.Id);
        }

        [Fact]
        public void patron_cannot_cancel_a_hold_which_does_not_exist()
        {
            // Given
            var bookOnHold = BookFixture.BookOnHold();
            var patron = PatronFixture.RegularPatron();

            // When
            var cancelHold = patron.CancelHold(bookOnHold);

            // Then
            cancelHold.Should().BeOfType<BookHoldCancelingFailed>();
        }

        [Fact]
        public void patron_cannot_cancel_a_hold_which_was_done_by_someone_else()
        {
            // Given
            var bookOnHold = BookFixture.BookOnHold();
            var patron = PatronFixture.RegularPatron();
            var differentPatron = PatronFixture.RegularPatronWithHold(PatronFixture.AnyPatronId, bookOnHold);

            // When
            var cancelHold = patron.CancelHold(bookOnHold);

            // Then
            cancelHold.Should().BeOfType<BookHoldCancelingFailed>();
        }
    }
}
