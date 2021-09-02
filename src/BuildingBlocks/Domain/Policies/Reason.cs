using System.Collections.Generic;

namespace Library.BuildingBlocks.Domain.Policies
{
    public class Reason : ValueObject
    {
        public string Value { get; }

        public Reason(string reason)
        {
            Value = reason;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
