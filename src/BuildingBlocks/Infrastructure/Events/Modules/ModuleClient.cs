using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Library.BuildingBlocks.Infrastructure.Events.Modules
{
    internal sealed class ModuleClient : IModuleClient
    {
        private readonly IModuleRegistry _moduleRegistry;

        public ModuleClient(IModuleRegistry moduleRegistry)
        {
            _moduleRegistry = moduleRegistry;
        }

        public async Task Publish(object message)
        {
            var key = message.GetType().Name;
            var registrations = _moduleRegistry.GetBroadcastRegistration(key);

            var tasks = new List<Task>();

            foreach (var registration in registrations)
            {
                var handle = registration.Handle;
                tasks.Add(handle(message));
            }

            await Task.WhenAll(tasks);
        }
    }
}
