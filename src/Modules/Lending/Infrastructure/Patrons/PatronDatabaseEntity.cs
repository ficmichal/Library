using System;
using System.Collections.Generic;
using System.Linq;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;

namespace Library.Modules.Lending.Infrastructure.Patrons
{
    public class PatronDatabaseEntity
    {
        public int Id { get; set; }

        public Guid PatronId { get; set; }

        public PatronType PatronType { get; set; }

        public ICollection<HoldDatabaseEntity> BooksOnHold { get; set; } = new List<HoldDatabaseEntity>();

        public PatronDatabaseEntity Handle(IPatronEvent @event)
        {
            return @event switch
            {
                BookPlacedOnHoldEvents bookPlacedOnHoldEvents => PlaceOnHold(bookPlacedOnHoldEvents),
                BookPlacedOnHold bookPlacedOnHold => PlaceOnHold(bookPlacedOnHold),
                BookHoldCanceled bookHoldCanceled => CancelHold(bookHoldCanceled),
                _ => this
            };
        }

        private PatronDatabaseEntity PlaceOnHold(BookPlacedOnHoldEvents events)
        {
            return PlaceOnHold(events.BookPlacedOnHold);
        }

        private PatronDatabaseEntity PlaceOnHold(BookPlacedOnHold @event)
        {
            BooksOnHold.Add(new HoldDatabaseEntity(@event.BookId, @event.PatronIdValue, @event.LibraryBranchId, @event.HoldTill));

            return this;
        }

        private PatronDatabaseEntity CancelHold(BookHoldCanceled @event)
        {
            RemoveHoldIfPresent(@event.PatronIdValue, @event.BookId, @event.LibraryBranchId);

            return this;
        }

        private PatronDatabaseEntity RemoveHoldIfPresent(Guid patronId, Guid bookId, Guid libraryBranchId)
        {
            var holdToRemove = BooksOnHold.FirstOrDefault(x => x.Is(bookId, patronId, libraryBranchId));
            if (holdToRemove is { })
            {
                BooksOnHold.Remove(holdToRemove);
            }

            return this;
        }
    }
}
