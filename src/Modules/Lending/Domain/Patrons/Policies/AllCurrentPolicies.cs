using System.Collections.Generic;

namespace Library.Modules.Lending.Domain.Patrons.Policies
{
    public static class AllCurrentPolicies
    {
        public static List<IPlacingOnHoldPolicy> Get()
            => new()
            {
                new RegularPatronMaximumNumberOfHoldsPolicy(),
                new OnlyResearcherPatronsCanPlaceOpenEndedHoldsPolicy(),
            };
    }
}
