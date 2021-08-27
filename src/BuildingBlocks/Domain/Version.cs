using System.Collections.Generic;

namespace Library.BuildingBlocks.Domain
{
    public class Version : ValueObject
    {
        public int Value { get; }

        public static Version Zero()
        {
            return new(0);
        }

        private Version(int version)
        {
            Value = version;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
