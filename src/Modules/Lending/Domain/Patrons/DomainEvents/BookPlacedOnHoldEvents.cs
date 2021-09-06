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

        public MaximumNumberOhHoldsReached MaximumNumberOhHoldsReached { get; }

        public static BookPlacedOnHoldEvents Events(BookPlacedOnHold bookPlacedOnHold)
        {
            return new(bookPlacedOnHold.PatronIdValue, bookPlacedOnHold);
        }

        public static BookPlacedOnHoldEvents Events(BookPlacedOnHold bookPlacedOnHold, MaximumNumberOhHoldsReached maximumNumberOhHoldsReached)
        {
            return new(bookPlacedOnHold.PatronIdValue, bookPlacedOnHold, maximumNumberOhHoldsReached);
        }

        public List<IDomainEvent> Normalize()
        {
            var events = new List<IDomainEvent> {BookPlacedOnHold};
            if (MaximumNumberOhHoldsReached is {})
            {
                events.Add(MaximumNumberOhHoldsReached);
            }

            return events;
        }

        private BookPlacedOnHoldEvents(Guid patronIdValue, BookPlacedOnHold bookPlacedOnHold,
            MaximumNumberOhHoldsReached maximumNumberOhHoldsReached = null)
        {
            PatronIdValue = patronIdValue;
            BookPlacedOnHold = bookPlacedOnHold;
            MaximumNumberOhHoldsReached = maximumNumberOhHoldsReached;
        }
    }
}
