using Library.BuildingBlocks.Domain;
using System;
using System.Collections.Generic;

namespace Library.Modules.Lending.Domain.Patrons.Hold
{
    public class HoldDuration : ValueObject
    {
        public DateTime From { get; }

        public DateTime? To { get; }

        private HoldDuration(DateTime @from, DateTime? to)
        {
            if (to < @from)
            {
                throw new ArgumentException("Duration must be valid");
            }

            From = @from;
            To = to;
        }

        public bool IsOpenEnded()
        {
            return !To.HasValue;
        }

        public static HoldDuration OpenEnded()
        {
            return OpenEnded(DateTime.Now);
        }

        public static HoldDuration OpenEnded(DateTime from)
        {
            return new(from, null);
        }

        public static HoldDuration CloseEnded(NumberOfDays days)
        {
            return CloseEnded(DateTime.Now, days);
        }

        public static HoldDuration CloseEnded(int days)
        {
            return CloseEnded(DateTime.Now, NumberOfDays.Of(days));
        }

        public static HoldDuration CloseEnded(DateTime from, NumberOfDays days)
        {
            var till = from + TimeSpan.FromDays(days.Days);

            return new HoldDuration(from, till);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return From;
            yield return To;
        }
    }
}
