using System.Threading.Tasks;

namespace Library.BuildingBlocks.Infrastructure.Events.Modules
{
    public interface IModuleClient
    {
        Task Publish(object message);
    }
}