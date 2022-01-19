using Library.BuildingBlocks.Domain.Policies;
using Library.Modules.Lending.Domain.Books.Types;
using Library.Modules.Lending.Domain.Patrons.Hold;

namespace Library.Modules.Lending.Domain.Patrons.Policies
{
    public class OnlyResearcherPatronsCanPlaceOpenEndedHoldsPolicy : IPlacingOnHoldPolicy
    {
        public IPolicyResult Check(AvailableBook book, Patron patron, HoldDuration holdDuration)
        {
            if (patron.IsRegular() && holdDuration.IsOpenEnded())
            {
                return Rejection.WithReason("Regular patron cannot place open ended holds.");
            }

            return new Allowance();
        }
    }
}
