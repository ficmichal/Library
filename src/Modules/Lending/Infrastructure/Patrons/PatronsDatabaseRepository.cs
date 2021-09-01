using Dapper;
using Library.BuildingBlocks.Domain.Events;
using Library.BuildingBlocks.Infrastructure.Data;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using Library.Modules.Lending.Infrastructure.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Modules.Lending.Infrastructure.Patrons
{
    public class PatronsDatabaseRepository : IPatronRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IDomainEvents _domainEvents;

        public PatronsDatabaseRepository(ISqlConnectionFactory sqlConnectionFactory, IDomainEvents domainEvents)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _domainEvents = domainEvents;
        }

        public Patron FindBy(PatronId patronId)
        {
            throw new NotImplementedException();
        }

        private async Task<PatronDatabaseEntity> FindByPatronId(PatronId patronId)
        {
            PatronDatabaseEntity patronEntry = default;

            var connection = _sqlConnectionFactory.GetOpenConnection();

            return (await connection.QueryAsync<PatronDatabaseEntity, HoldDatabaseEntity, PatronDatabaseEntity>(
                "SELECT " +
                $"[Patron].[Id] AS [{nameof(PatronDatabaseEntity.Id)}], " +
                $"[Patron].[PatronId] AS [{nameof(PatronDatabaseEntity.PatronId)}], " +
                $"[Patron].[PatronType] AS [{nameof(PatronDatabaseEntity.PatronType)}] " +
                $"FROM {nameof(PatronDatabaseEntity)} AS [Patron] " +
                $"INNER JOIN ON {nameof(HoldDatabaseEntity)} AS [Hold] ON [Patron].[PatronId] = [Hold].[PatronId] " +
                "WHERE [Patron].[PatronId] = @PatronId",
                (patron, hold) =>
                {
                    patronEntry ??= patron;
                    patronEntry.BooksOnHold.Add(hold);

                    return patronEntry;
                },
                new
                {
                    PatronId = patronId.Id
                })).FirstOrDefault();
        }

        public Patron Publish(IPatronEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
