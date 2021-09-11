using System;
using System.Threading.Tasks;
using Library.BuildingBlocks.Infrastructure.Events;
using Library.Modules.Lending.Application.Patrons.Hold;
using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Books.DomainEvents;
using Library.Modules.Lending.Domain.Patrons;

namespace Library.Modules.Lending.Application.Books.EventListeners
{
    public class BookDuplicateHoldFoundListener : IEventListener<BookDuplicateHoldFound>
    {
        private readonly CancelingHold _cancelingHold;

        public BookDuplicateHoldFoundListener(CancelingHold cancelingHold)
        {
            _cancelingHold = cancelingHold;
        }

        public async Task HandleAsync(BookDuplicateHoldFound @event)
        {
            _ =  await _cancelingHold.CancelHold(CancelHoldCommandFrom(@event));
        }

        private CancelHoldCommand CancelHoldCommandFrom(BookDuplicateHoldFound @event)
        {
            return new CancelHoldCommand(DateTime.Now, new PatronId(@event.SecondPatronId), new BookId(@event.BookId));
        }
    }
}
