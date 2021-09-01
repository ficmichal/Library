using Library.BuildingBlocks.Domain.Events;
using System.Threading.Channels;

namespace Library.BuildingBlocks.Infrastructure.Events
{
    public interface IEventChannel
    {
        ChannelReader<IDomainEvent> Reader { get; }
        ChannelWriter<IDomainEvent> Writer { get; }
    }
}