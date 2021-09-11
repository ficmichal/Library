using System;

namespace Library.Modules.Lending.Domain.Patrons.DomainEvents
{
    public class MaximumNumberOhHoldsReached : IPatronEvent
    {
        public Guid EventId => Guid.NewGuid();
        public DateTime When { get; }
        public Guid PatronIdValue { get; }
        public int NumberOfHolds { get; }

        public static MaximumNumberOhHoldsReached Now(PatronInformation patronInformation, int numberOfHolds)
        {
            return new(patronInformation.PatronId.Id, numberOfHolds);
        }
        
        private MaximumNumberOhHoldsReached(Guid patronIdValue, int numberOfHolds)
        {
            When = DateTime.Now;
            PatronIdValue = patronIdValue;
            NumberOfHolds = numberOfHolds;
        }
    }
}
