using Library.BuildingBlocks.Domain.Events;
using System.Threading.Tasks;

namespace Library.BuildingBlocks.Infrastructure.Events.Dispatchers
{
    public class AsynchronousEventDispatcher : IAsynchronousDispatcher
    {
        private readonly IEventChannel _eventChannel;

        public AsynchronousEventDispatcher(IEventChannel eventChannel)
        {
            _eventChannel = eventChannel;
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IDomainEvent
        {
            await _eventChannel.Writer.WriteAsync(@event);
        }
    }
}
