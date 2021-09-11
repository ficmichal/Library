using Library.BuildingBlocks.Domain.Events;
using Library.BuildingBlocks.Infrastructure.Events;
using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Books.DomainEvents;
using Library.Modules.Lending.Domain.Books.Types;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using System.Threading.Tasks;

namespace Library.Modules.Lending.Application.Books.EventListeners
{
    public class BookPlacedOnHoldListener : IEventListener<BookPlacedOnHold>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IDomainEvents _domainEvents;

        public BookPlacedOnHoldListener(IBookRepository bookRepository, IDomainEvents domainEvents)
        {
            _bookRepository = bookRepository;
            _domainEvents = domainEvents;
        }

        public async Task HandleAsync(BookPlacedOnHold @event)
        {
            var book = await _bookRepository.FindBy(new BookId(@event.BookId));

            var handledBook = book switch
            {
                AvailableBook availableBook => availableBook.Handle(@event),
                BookOnHold bookOnHold => RaiseDuplicateHoldFoundEvent(bookOnHold, @event),
                _ => book
            };

            await _bookRepository.Save(handledBook);
        }

        private BookOnHold RaiseDuplicateHoldFoundEvent(BookOnHold onHold, BookPlacedOnHold bookPlacedOnHold)
        {
            if (onHold.By(new PatronId(bookPlacedOnHold.PatronIdValue)))
            {
                return onHold;
            }

            _domainEvents.Publish(
                BookDuplicateHoldFound.Now(
                    onHold.ByPatron.Id,
                    bookPlacedOnHold.PatronIdValue,
                    bookPlacedOnHold.LibraryBranchId,
                    bookPlacedOnHold.BookId));

            return onHold;
        }
    }
}
