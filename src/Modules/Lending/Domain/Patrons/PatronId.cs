using Library.BuildingBlocks.Domain;
using System;
using System.Collections.Generic;

namespace Library.Modules.Lending.Domain.Patrons
{
    public class PatronId : ValueObject
    {
        public PatronId(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
