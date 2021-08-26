using System;

namespace Library.BuildingBlocks.Domain.Events
{
    public interface IDomainEvent
    {
        Guid EventId { get; }

        Guid AggregateId { get; }

        DateTime When { get; }
    }
}