using Library.BuildingBlocks.Domain;
using System;
using System.Collections.Generic;

namespace Library.Modules.Lending.Domain.LibraryBranch
{
    public class LibraryBranchId : ValueObject
    {
        public LibraryBranchId(Guid id)
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
