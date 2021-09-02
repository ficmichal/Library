using System.Collections.Generic;

namespace Library.BuildingBlocks.Domain.Policies
{
    public class Rejection : ValueObject, IPolicyResult
    {
        public Reason Reason { get; }

        public static Rejection WithReason(string reason)
        {
            return new(new Reason(reason));
        }

        private Rejection(Reason reason)
        {
            Reason = reason;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Reason;
        }
    }
}
