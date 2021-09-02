using System.Collections.Generic;

namespace Library.Modules.Lending.Domain.Patrons
{
    public class PatronHolds
    {
        public const int MaximumNumberOfHolds = 5;

        private readonly ISet<Hold.Hold> _resourcesOnHold;

        public PatronHolds(ISet<Hold.Hold> resourcesOnHold)
        {
            _resourcesOnHold = resourcesOnHold;
        }

        public int Count => _resourcesOnHold.Count;
    }
}
