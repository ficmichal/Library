using System;

namespace Library.BuildingBlocks.Domain.Exceptions
{
    public class AggregateRootIsStale : Exception
    {
        public AggregateRootIsStale(string message) : base(message) {}
    }
}
