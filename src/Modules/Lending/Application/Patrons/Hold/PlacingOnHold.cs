using Library.BuildingBlocks.Domain.Commands;
using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Books.Types;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using System;
using System.Threading.Tasks;

namespace Library.Modules.Lending.Application.Patrons.Hold
{
    public class PlacingOnHold
    {
        private readonly IBookRepository _bookRepository;
        private readonly IPatronRepository _patronRepository;

        public PlacingOnHold(IBookRepository bookRepository, IPatronRepository patronRepository)
        {
            _bookRepository = bookRepository;
            _patronRepository = patronRepository;
        }

        public async Task<Result> PlaceOnHold(PlaceOnHoldCommand command)
        {
            var availableBook = await FindAvailableBook(command.BookId);
            var patron = await FindPatron(command.PatronId);
            var result = patron.PlaceOnHold(availableBook, command.GetHoldDuration());

            return result switch
            {
                BookHoldFailed bookHoldFailed => await PublishEvents(bookHoldFailed),
                BookPlacedOnHoldEvents bookPlacedOnHoldEvents => await PublishEvents(bookPlacedOnHoldEvents),
            };
        }

        private async Task<Result> PublishEvents(BookPlacedOnHoldEvents @event)
        {
            await _patronRepository.Publish(@event);
            return Result.Success;
        }

        private async Task<Result> PublishEvents(BookHoldFailed @event)
        {
            await _patronRepository.Publish(@event);
            return Result.Rejection;
        }

        private async Task<AvailableBook> FindAvailableBook(BookId id)
        {
            var book = await _bookRepository.FindBy(id);

            if (book is not AvailableBook availableBook)
            {
                throw new ArgumentException($"Cannot find available book with Id: {id.Id}");
            }

            return availableBook;
        }

        private async Task<Patron> FindPatron(PatronId id)
        {
            var patron = await _patronRepository.FindBy(id);

            if (patron is null)
            {
                throw new ArgumentException($"Patron with given Id does not exists: {id.Id}");

            }

            return patron;
        }
    }
}
