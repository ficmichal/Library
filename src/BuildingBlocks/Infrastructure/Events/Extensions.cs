using Library.BuildingBlocks.Domain.Events;
using Library.BuildingBlocks.Infrastructure.Events.Dispatchers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Library.BuildingBlocks.Infrastructure.Events
{
    public static class Extensions
    {
        public static IServiceCollection AddEventDispatching(this IServiceCollection services)
        {
            services.AddSingleton<IDomainEvents, JustForwardDomainEventPublisher>();
            services.AddSingleton<IEventChannel, EventChannel>();
            services.AddSingleton<IAsynchronousDispatcher, AsynchronousEventDispatcher>();

            services.AddHostedService<AsynchronousEventDispatcherJob>();

            return services;
        }

        public static IServiceCollection AddEvents(this IServiceCollection services)
        {
            services.AddSingleton<IEventDispatcher, EventDispatcher>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            services.Scan(s => s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IEventListener<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}
