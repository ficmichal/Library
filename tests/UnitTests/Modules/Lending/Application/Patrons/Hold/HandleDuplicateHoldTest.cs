using System;
using System.Threading.Tasks;
using Library.Modules.Lending.Application.Books.EventListeners;
using Library.Modules.Lending.Application.Patrons.Hold;
using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Books.DomainEvents;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Books;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.LibraryBranches;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Patrons;
using NSubstitute;
using Xunit;

namespace Library.Modules.Lending.Application.UnitTests.Patrons.Hold
{
    public class HandleDuplicateHoldTest
    {
        private readonly DateTime _fixedDate = new(2020, 7, 7);

        [Fact]
        public async Task should_start_cancelling_hold_if_book_was_already_hold_by_other_patron()
        {
            var cancelingHold = CancelingHold();
            var duplicateHold = new BookDuplicateHoldFoundListener(cancelingHold, _fixedDate);
            var bookDuplicateHoldFound = DuplicateHoldFoundBy();
            var cancelHoldCommand = CancelHoldCommandFrom(bookDuplicateHoldFound);

            await duplicateHold.HandleAsync(bookDuplicateHoldFound);

            await cancelingHold.Received(1).CancelHold(cancelHoldCommand);
        }


        private static ICancelingHold CancelingHold()
        {
            var substitute = Substitute
                .For<ICancelingHold>();

            return substitute;
        }

        BookDuplicateHoldFound DuplicateHoldFoundBy()
        {
            return BookDuplicateHoldFound.Create(
                    _fixedDate,
                    PatronFixture.AnyPatronId.Id,
                    PatronFixture.AnyPatronId.Id,
                    LibraryBranchFixture.AnyBranchId.Id,
                    BookFixture.AnyBookId.Id);
        }

        CancelHoldCommand CancelHoldCommandFrom(BookDuplicateHoldFound @event)
        {
            return new CancelHoldCommand(
                _fixedDate,
                new PatronId(@event.SecondPatronId),
                new BookId(@event.BookId));
        }
    }
}
