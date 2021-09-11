using Library.BuildingBlocks.Infrastructure.Events;
using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Modules.Lending.Application.Books.EventListeners
{
    public class BookHoldCanceledListener : IEventListener<BookHoldCanceled>
    {
        private readonly IBookRepository _bookRepository;

        public BookHoldCanceledListener(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task HandleAsync(BookHoldCanceled @event)
        {
            var book = await _bookRepository.FindBy(new BookId(@event.BookId));
        }
    }
}
