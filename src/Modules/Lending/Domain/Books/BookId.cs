using System;

namespace Library.Modules.Lending.Domain.Books
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
