using Library.Modules.Lending.Domain.Books.Types;
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

        public bool MaximumHoldsAfterHolding()
        {
            return Count + 1 == MaximumNumberOfHolds;
        }

        public bool A(BookOnHold bookOnHold)
        {
            var hold = new Hold.Hold(bookOnHold.Id, bookOnHold.HoldPlacedAt);

            return _resourcesOnHold.Contains(hold);
        }
    }
}
