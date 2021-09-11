using Library.BuildingBlocks.Domain.Commands;
using Library.BuildingBlocks.Infrastructure.Events;
using Library.Modules.Lending.Application.Patrons.Hold;
using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Books.DomainEvents;
using Library.Modules.Lending.Domain.Patrons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Modules.Lending.Application.Books.EventListeners
{
    public class BookDuplicateHoldFoundListener : IEventListener<BookDuplicateHoldFound>
    {
        private readonly CancelingHold _cancelingHold;

        public BookDuplicateHoldFoundListener(CancelingHold cancelingHold)
        {
            _cancelingHold = cancelingHold;
        }

        public async Task<Result> HandleAsync(BookDuplicateHoldFound @event)
        {
            return await _cancelingHold.CancelHold(CancelHoldCommandFrom(@event));
        }

        private CancelHoldCommand CancelHoldCommandFrom(BookDuplicateHoldFound @event)
        {
            return new CancelHoldCommand(DateTime.Now, new PatronId(@event.SecondPatronId), new BookId(@event.BookId));
        }
    }
}
