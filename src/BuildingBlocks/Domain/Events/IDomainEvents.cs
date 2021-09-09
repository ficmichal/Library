using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.BuildingBlocks.Domain.Events
{
    public interface IDomainEvents
    {
        Task Publish<TEvent>(TEvent @event) where TEvent : class, IDomainEvent;

        async Task Publish(IEnumerable<IDomainEvent> events)
        {
            foreach (var domainEvent in events)
            {
                await Publish(domainEvent);
            }
        }
    }
}