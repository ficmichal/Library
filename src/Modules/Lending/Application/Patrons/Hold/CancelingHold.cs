using Library.BuildingBlocks.Domain.Commands;
using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Books.Types;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using System.Threading.Tasks;

namespace Library.Modules.Lending.Application.Patrons.Hold
{
    public class CancelingHold : ICancelingHold
    {
        private readonly IBookRepository _bookRepository;
        private readonly IPatronRepository _patronRepository;

        public CancelingHold(IBookRepository bookRepository, IPatronRepository patronRepository)
        {
            _bookRepository = bookRepository;
            _patronRepository = patronRepository;
        }

        public async Task<Result> CancelHold(CancelHoldCommand command)
        {
            var bookOnHold = await Find(command.BookId);
            var patron = await Find(command.PatronId);

            var result = patron.CancelHold(bookOnHold);

            return result switch
            {
                BookHoldCanceled bookHoldCanceled => await PublishEvents(bookHoldCanceled),
                BookHoldCancelingFailed bookHoldCancelingFailed => await PublishEvents(bookHoldCancelingFailed),
            };
        }

        private async Task<Result> PublishEvents(BookHoldCanceled @event)
        {
            await _patronRepository.Publish(@event);
            return Result.Success;
        }

        private async Task<Result> PublishEvents(BookHoldCancelingFailed @event)
        {
            await _patronRepository.Publish(@event);
            return Result.Rejection;
        }

        private async Task<BookOnHold> Find(BookId bookId)
        {
            return await _bookRepository.FindBy(bookId) as BookOnHold;
        }

        private async Task<Patron> Find(PatronId patronId)
        {
            return await _patronRepository.FindBy(patronId);
        }
    }
}
