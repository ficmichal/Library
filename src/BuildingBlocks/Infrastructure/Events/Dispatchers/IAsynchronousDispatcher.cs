using Library.BuildingBlocks.Domain.Events;
using System.Threading.Tasks;

namespace Library.BuildingBlocks.Infrastructure.Events.Dispatchers
{
    public interface IAsynchronousDispatcher
    {
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IDomainEvent;
    }
}