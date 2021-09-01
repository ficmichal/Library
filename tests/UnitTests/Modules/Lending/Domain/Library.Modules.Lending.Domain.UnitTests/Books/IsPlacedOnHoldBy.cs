using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using Library.Modules.Lending.Domain.Patrons.Hold;
using System;

namespace Library.Modules.Lending.Domain.UnitTests.Books
{
    public class PlaceOnHold
    {
        private readonly IBook _book;

        private PatronId _onHoldPatronId;

        private LibraryBranchId _placeOnHoldBranchId;

        private DateTime _onHoldFrom;

        private DateTime _onHoldTo;

        private PlaceOnHold(Func<IBook> bookProvider)
        {
            _book = bookProvider.Invoke();
            _onHoldFrom = DateTime.Now;
        }

        public static PlaceOnHold The(Func<IBook> bookProvider)
        {
            return new(bookProvider);
        }

        public PlaceOnHold By(PatronId onHoldPatronId)
        {
            _onHoldPatronId = onHoldPatronId;

            return this;
        }

        public PlaceOnHold At(LibraryBranchId branchId)
        {
            _placeOnHoldBranchId = branchId;

            return this;
        }

        public PlaceOnHold From(DateTime from)
        {
            _onHoldFrom = from;

            return this;
        }

        public PlaceOnHold Till(DateTime to)
        {
            _onHoldTo = to;

            return this;
        }

        public BookPlacedOnHold Place()
        {
            return BookPlacedOnHoldNow(_book, _onHoldPatronId, _placeOnHoldBranchId, _onHoldFrom, _onHoldTo);
        }

        private static BookPlacedOnHold BookPlacedOnHoldNow(IBook availableBook, PatronId byPatron, LibraryBranchId libraryBranchId,
            DateTime from, DateTime till)
        {
            return BookPlacedOnHold.BookPlacedOnHoldNow(byPatron, availableBook.Id, availableBook.Type, libraryBranchId,
                HoldDuration.CloseEnded(from, NumberOfDays.Of((till - from).Days)));
        }
    }
}
