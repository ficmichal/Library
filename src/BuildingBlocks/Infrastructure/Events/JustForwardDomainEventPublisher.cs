using Library.BuildingBlocks.Domain.Events;
using Library.BuildingBlocks.Infrastructure.Events.Dispatchers;
using System.Threading.Tasks;

namespace Library.BuildingBlocks.Infrastructure.Events
{
    public class JustForwardDomainEventPublisher : IDomainEvents
    {
        private readonly IAsynchronousDispatcher _asynchronousDispatcher;

        public JustForwardDomainEventPublisher(IAsynchronousDispatcher asynchronousDispatcher)
        {
            _asynchronousDispatcher = asynchronousDispatcher;
        }

        public async Task Publish<TEvent>(TEvent @event) where TEvent : class, IDomainEvent
        {
            if (@event is null) return;

            await _asynchronousDispatcher.PublishAsync(@event);
        }
    }
}
