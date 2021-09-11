using System;

namespace Library.BuildingBlocks.Domain.Events
{
    public interface IDomainEvent
    {
        Guid EventId => Guid.NewGuid();

        Guid AggregateId { get; }

        DateTime When { get; }
    }
}