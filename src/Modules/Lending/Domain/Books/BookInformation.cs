namespace Library.Modules.Lending.Domain.Books
{
    public class BookInformation
    {
        public BookInformation(BookId id, BookType type)
        {
            Id = id;
            Type = type;
        }

        public BookId Id { get; }

        public BookType Type { get; }
    }
}
