using System;

namespace Library.Modules.Lending.Domain.Patrons.DomainEvents
{
    public class PatronCreated : IPatronEvent
    {
        public Guid EventId => Guid.NewGuid();
        public DateTime When { get; }
        public Guid PatronIdValue { get; }
        public PatronType PatronType { get; }

        public static PatronCreated Now(PatronId patronId, PatronType patronType)
        {
            return new(patronId, patronType);
        }

        private PatronCreated(PatronId patronId, PatronType patronType)
        {
            When = DateTime.Now;
            PatronIdValue = patronId.Id;
            PatronType = patronType;
        }
    }
}
