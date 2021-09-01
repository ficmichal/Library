using Library.Modules.Lending.Domain.Patrons;

namespace Library.Modules.Lending.Infrastructure.Patrons
{
    public class DomainModelMapper
    {
        public Patron Map(PatronDatabaseEntity entity)
        {
            return PatronFactory.Create(entity.PatronType, new PatronId(entity.PatronId));
        }
    }
}
