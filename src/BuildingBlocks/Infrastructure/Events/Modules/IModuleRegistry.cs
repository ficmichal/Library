using System.Collections.Generic;

namespace Library.BuildingBlocks.Infrastructure.Events.Modules
{
    public interface IModuleRegistry
    {
        IEnumerable<ModuleBroadcastRegistration> GetBroadcastRegistration(string key);
        void AddBroadcastRegistration(ModuleBroadcastRegistration registration);
    }
}