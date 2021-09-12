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
        private readonly ICancelingHold _cancelingHold;
        private readonly DateTime _dateTime;

        public BookDuplicateHoldFoundListener(ICancelingHold cancelingHold)
        {
            _cancelingHold = cancelingHold;
            _dateTime = DateTime.Now;
        }

        public BookDuplicateHoldFoundListener(ICancelingHold cancelingHold, DateTime dateTime)
        {
            _cancelingHold = cancelingHold;
            _dateTime = dateTime;
        }

        public async Task HandleAsync(BookDuplicateHoldFound @event)
        {
            _ =  await _cancelingHold.CancelHold(CancelHoldCommandFrom(@event));
        }

        private CancelHoldCommand CancelHoldCommandFrom(BookDuplicateHoldFound @event)
        {
            return new CancelHoldCommand(_dateTime, new PatronId(@event.SecondPatronId), new BookId(@event.BookId));
        }
    }
}
