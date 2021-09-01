using Library.BuildingBlocks.Infrastructure.Events.Modules;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Library.BuildingBlocks.Infrastructure.Events.Dispatchers
{
    public class AsynchronousEventDispatcherJob : BackgroundService
    {
        private readonly IEventChannel _eventChannel;
        private readonly IModuleClient _moduleClient;

        public AsynchronousEventDispatcherJob(IEventChannel eventChannel, IModuleClient moduleClient)
        {
            _eventChannel = eventChannel;
            _moduleClient = moduleClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var message in _eventChannel.Reader.ReadAllAsync(stoppingToken))
            {
                await _moduleClient.Publish(message);
            }
        }
    }
}
