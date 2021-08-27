using Library.BuildingBlocks.Domain;
using System.Collections.Generic;

namespace Library.Modules.Lending.Domain.Patrons.Hold
{
    public class NumberOfDays : ValueObject
    {
        public int Days { get; }

        private NumberOfDays(int days)
        {
            Days = days;
        }

        public static NumberOfDays Of(int days)
        {
            return new(days);
        }

        public bool IsGreaterThan(int days)
        {
            return Days > days;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Days;
        }
    }
}
