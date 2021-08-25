using Library.BuildingBlocks.Domain;
using System.Collections.Generic;

namespace Library.Modules.Lending.Domain.Books
{
    public class BookInformation : ValueObject
    {
        public BookInformation(BookId id, BookType type)
        {
            Id = id;
            Type = type;
        }

        public BookId Id { get; }

        public BookType Type { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
            yield return Type;
        }
    }
}
