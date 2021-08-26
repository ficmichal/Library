using Library.BuildingBlocks.Domain.Events;
using System;
using System.Collections.Generic;

namespace Library.Modules.Lending.Domain.Patrons.Events
{
    public interface IPatronEvent : IDomainEvent
    {
        PatronId PatronId => new(PatronIdValue);

        Guid PatronIdValue { get; }

        List<IDomainEvent> Normalize() => new() {this};
    }
}