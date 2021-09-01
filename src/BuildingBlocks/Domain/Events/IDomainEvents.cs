using System.Collections.Generic;

namespace Library.BuildingBlocks.Domain.Events
{
    public interface IDomainEvents
    {
        void Publish(IDomainEvent @event);

        void Publish(IEnumerable<IDomainEvent> events)
        {
            foreach (var domainEvent in events)
            {
                Publish(domainEvent);
            }
        }
    }
}