using Library.BuildingBlocks.Domain.Policies;
using Library.Modules.Lending.Domain.Books.Types;
using Library.Modules.Lending.Domain.Patrons.Hold;

namespace Library.Modules.Lending.Domain.Patrons.Policies
{
    public interface IPlacingOnHoldPolicy
    {
        public IPolicyResult Check(AvailableBook book, Patron patron, HoldDuration holdDuration);
    }
}
