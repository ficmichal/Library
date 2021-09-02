using System.Collections.Generic;

namespace Library.BuildingBlocks.Domain.Policies
{
    public class Allowance : ValueObject, IPolicyResult
    {
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return 0;
        }
    }
}
