using Library.BuildingBlocks.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Library.BuildingBlocks.Infrastructure.Events.Dispatchers
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public EventDispatcher(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IDomainEvent
        {
            using var scope = _serviceProvider.CreateScope();
            var handlers = scope.ServiceProvider.GetServices<IEventListener<TEvent>>();

            var tasks = handlers.Select(x => x.HandleAsync(@event));

            await Task.WhenAll(tasks);
        }
    }
}
