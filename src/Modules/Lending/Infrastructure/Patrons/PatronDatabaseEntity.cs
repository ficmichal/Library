using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using System;
using System.Collections.Generic;

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
                BookPlacedOnHold bookPlacedOnHold => PlaceOnHold(bookPlacedOnHold)
            };
        }

        public PatronDatabaseEntity PlaceOnHold(BookPlacedOnHoldEvents events)
        {
            return PlaceOnHold(events.BookPlacedOnHold);
        }

        public PatronDatabaseEntity PlaceOnHold(BookPlacedOnHold @event)
        {
            BooksOnHold.Add(new HoldDatabaseEntity(@event.BookId, @event.PatronIdValue, @event.LibraryBranchId, @event.HoldTill));

            return this;
        }
    }
}
