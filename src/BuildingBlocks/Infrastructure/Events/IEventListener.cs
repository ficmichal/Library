using Library.BuildingBlocks.Domain.Events;
using System.Threading.Tasks;

namespace Library.BuildingBlocks.Infrastructure.Events
{
    public interface IEventListener<in TEvent> where TEvent : class, IDomainEvent
    {
        Task HandleAsync(TEvent @event);
    }
}