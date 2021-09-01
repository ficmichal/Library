using Library.BuildingBlocks.Domain.Events;
using System.Threading.Channels;

namespace Library.BuildingBlocks.Infrastructure.Events
{
    public class EventChannel : IEventChannel
    {
        private readonly Channel<IDomainEvent> _messages = Channel.CreateUnbounded<IDomainEvent>();

        public ChannelReader<IDomainEvent> Reader => _messages.Reader;
        public ChannelWriter<IDomainEvent> Writer => _messages.Writer;
    }
