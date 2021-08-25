using Library.BuildingBlocks.Domain;
using System;
using System.Collections.Generic;

namespace Library.Modules.Lending.Domain.Books
{
    public class BookId : ValueObject
    {
        public BookId(Guid id)
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
