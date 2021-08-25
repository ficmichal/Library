using System;

namespace Library.Modules.Catalogue.Domain.Books
{
    public class BookInstance
    {
        public ISBN ISBN { get; }

        public BookId Id { get; }

        public BookType Type { get; }

        public static BookInstance InstanceOf(Book book, BookType bookType)
        {
            return new(book.ISBN, new BookId(Guid.NewGuid()), bookType);
        }

        private BookInstance(ISBN isbn, BookId id, BookType type)
        {
            ISBN = isbn;
            Id = id;
            Type = type;
        }
    }
}
