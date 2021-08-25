using Library.BuildingBlocks.Domain;

namespace Library.Modules.Lending.Domain.Books
{
    public interface IBook
    {
        BookId Id { get; }

        BookType Type { get; }

        BookInformation Information { get; }
        
        Version Version { get; }
    }
}
