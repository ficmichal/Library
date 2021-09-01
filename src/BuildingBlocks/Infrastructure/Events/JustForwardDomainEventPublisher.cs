using Library.BuildingBlocks.Domain.Events;
using Library.BuildingBlocks.Infrastructure.Events.Dispatchers;
using System.Threading.Tasks;

namespace Library.BuildingBlocks.Infrastructure.Events
{
    public class JustForwardDomainEventPublisher : IDomainEvents
    {
        private readonly IEventDispatcher _eventDispatcher;

        public JustForwardDomainEventPublisher(IEventDispatcher eventDispatcher)
        {
            _eventDispatcher = eventDispatcher;
        }

        public async Task Publish(IDomainEvent @event)
        {
            await _eventDispatcher.PublishAsync(@event);
        }
    }
}
