using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using System.Threading.Tasks;

namespace Library.Modules.Lending.Domain.Patrons
{
    public interface IPatronRepository
    {
        Task<Patron> FindBy(PatronId patronId);

        Task<Patron> Publish(IPatronEvent @event);
    }
}