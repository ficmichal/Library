using Library.BuildingBlocks.Domain.Events;
using System;
using System.Collections.Generic;

namespace Library.Modules.Lending.Domain.Patrons.DomainEvents
{
    public class BookPlacedOnHoldEvents : IPatronEvent
    {
        public Guid EventId => Guid.NewGuid();
        
        public Guid PatronIdValue { get; }

        public BookPlacedOnHold BookPlacedOnHold { get; }

        public DateTime When => BookPlacedOnHold.When;

        public static BookPlacedOnHoldEvents Events(BookPlacedOnHold bookPlacedOnHold)
        {
            return new(bookPlacedOnHold.PatronIdValue, bookPlacedOnHold);
        }

        public List<IDomainEvent> Normalize()
        {
            return new() {BookPlacedOnHold};
        }

        private BookPlacedOnHoldEvents(Guid patronIdValue, BookPlacedOnHold bookPlacedOnHold)
        {
            PatronIdValue = patronIdValue;
            BookPlacedOnHold = bookPlacedOnHold;
        }

    }
}
