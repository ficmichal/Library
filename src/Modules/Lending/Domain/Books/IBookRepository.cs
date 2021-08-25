using System.Threading.Tasks;

namespace Library.Modules.Lending.Domain.Books
{
    public interface IBookRepository
    {
        Task<IBook> FindBy(BookId id);

        Task Save(IBook book);
    }
}