using Library.Modules.Lending.Domain.Patrons.DomainEvents;

namespace Library.Modules.Lending.Domain.Patrons
{
    public interface IPatronRepository
    {
        Patron FindBy(PatronId patronId);

        Patron Publish(IPatronEvent @event);
    }
}