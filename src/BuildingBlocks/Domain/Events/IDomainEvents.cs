using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.BuildingBlocks.Domain.Events
{
    public interface IDomainEvents
    {
        Task Publish(IDomainEvent @event);

        async Task Publish(IEnumerable<IDomainEvent> events)
        {
            foreach (var domainEvent in events)
            {
                await Publish(domainEvent);
            }
        }
    }
}