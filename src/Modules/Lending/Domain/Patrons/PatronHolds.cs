using System.Collections.Generic;

namespace Library.Modules.Lending.Domain.Patrons
{
    public class PatronHolds
    {
        private readonly ISet<Hold.Hold> _resourcesOnHold;

        public PatronHolds(ISet<Hold.Hold> resourcesOnHold)
        {
            _resourcesOnHold = resourcesOnHold;
        }

        public int Count => _resourcesOnHold.Count;
    }
}
