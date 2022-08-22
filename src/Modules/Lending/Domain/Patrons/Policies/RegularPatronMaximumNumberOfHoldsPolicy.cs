using Library.BuildingBlocks.Domain.Policies;
using Library.Modules.Lending.Domain.Books.Types;
using Library.Modules.Lending.Domain.Patrons.Hold;

namespace Library.Modules.Lending.Domain.Patrons.Policies
{
    public class RegularPatronMaximumNumberOfHoldsPolicy : IPlacingOnHoldPolicy
    {
        public IPolicyResult Check(AvailableBook book, Patron patron, HoldDuration holdDuration)
        {
            if (patron.IsRegular() && patron.NumberOfHolds() >= PatronHolds.MaximumNumberOfHolds)
            {
                return Rejection.WithReason("Patron cannot hold more books.");
            }

            return new Allowance();
        }
    }
}
