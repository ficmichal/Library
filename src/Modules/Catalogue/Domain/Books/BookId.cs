using System;

namespace Library.Modules.Catalogue.Domain.Books
{
    public class BookId
    {
        public BookId(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
