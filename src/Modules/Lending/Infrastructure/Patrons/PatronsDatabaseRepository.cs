using Library.BuildingBlocks.Domain.Events;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static Library.Modules.Lending.Infrastructure.Patrons.DomainModelMapper;

namespace Library.Modules.Lending.Infrastructure.Patrons
{
    public class PatronsDatabaseRepository : IPatronRepository
    {
        private readonly PatronsDbContext _patronsDbContext;
        private readonly IDomainEvents _domainEvents;

        public PatronsDatabaseRepository(PatronsDbContext patronsDbContext, IDomainEvents domainEvents)
        {
            _patronsDbContext = patronsDbContext;
            _domainEvents = domainEvents;
        }

        public async Task<Patron> FindBy(PatronId patronId)
        {
            var patronEntry = await FindByPatronId(patronId);

            return Map(patronEntry);
        }

        private async Task<PatronDatabaseEntity> FindByPatronId(PatronId patronId)
        {
            return await _patronsDbContext.Patrons.FirstOrDefaultAsync(x => x.PatronId == patronId.Id);
        }

        public async Task<Patron> Publish(IPatronEvent @event)
        {
            var patron = await HandleNextEvent(@event);

            await _domainEvents.Publish(@event.Normalize());

            return patron;
        }

        private async Task<Patron> HandleNextEvent(IPatronEvent @event)
        {
            var entity = await FindByPatronId(@event.PatronId);
            entity = entity.Handle(@event);
            await Save();

            return Map(entity);
        }

        private async Task Save()
        {
            await _patronsDbContext.SaveChangesAsync();
        }
    }
}
