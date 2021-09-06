using FluentAssertions;
using Library.BuildingBlocks.Infrastructure.Events;
using Library.BuildingBlocks.Infrastructure.Events.Dispatchers;
using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using Library.Modules.Lending.Domain.Patrons.Hold;
using Library.Modules.Lending.Infrastructure.Patrons;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Books;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Data;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.LibraryBranches;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Patrons;
using System.Threading.Tasks;
using Xunit;
using static Library.Modules.Lending.Domain.Patrons.DomainEvents.PatronCreated;

namespace Library.Modules.Lending.IntegrationTests.Patrons
{
    public class PatronDatabaseRepositoryIT
    {
        private static readonly PatronId PatronId = PatronFixture.AnyPatronId;
        private static readonly PatronType Regular = PatronType.Regular;
        private static readonly LibraryBranchId LibraryBranchId = LibraryBranchFixture.AnyBranchId;
        private const string ConnectionString = DbFixture.ConnectionString;

        private readonly PatronsDatabaseRepository _repo =
            new(new PatronsDbContext(ConnectionString), new JustForwardDomainEventPublisher(
                new AsynchronousEventDispatcher(new EventChannel())));

        [Fact]
        public async Task persistence_in_real_database_should_work()
        {
            // When
            await _repo.Publish(PatronCreated());

            // Then
            await PatronShouldBeFoundInDatabaseWithZeroBooksOnHold(PatronId);

            // When
            await _repo.Publish(PlacedOnHold());

            // Then
            await PatronShouldBeFoundInDatabaseWithOneBookOnHold(PatronId);
        }

        private async Task PatronShouldBeFoundInDatabaseWithZeroBooksOnHold(PatronId patronId)
        {
            var patron = await LoadPersistedPatron(patronId);

            patron.NumberOfHolds().Should().Be(0);
            AssertPatronInformation(patron, patronId);
        }

        private async Task PatronShouldBeFoundInDatabaseWithOneBookOnHold(PatronId patronId)
        {
            var patron = await LoadPersistedPatron(patronId);

            patron.NumberOfHolds().Should().Be(1);
            AssertPatronInformation(patron, patronId);
        }

        private void AssertPatronInformation(Patron patron, PatronId patronId)
        {
            patron.Should().Be(PatronFixture.RegularPatron(patronId));
        }

        private async Task<Patron> LoadPersistedPatron(PatronId patronId)
        {
            var loaded = await _repo.FindBy(patronId);

            return loaded;
        }

        private PatronCreated PatronCreated()
        {
            return Now(PatronId, Regular);
        }

        private BookPlacedOnHold PlacedOnHold()
        {
            return
                BookPlacedOnHold.BookPlacedOnHoldNow(PatronId,
                    BookFixture.AnyBookId, BookType.Circulating,
                    LibraryBranchId,
                    HoldDuration.CloseEnded(5));
        }
    }
}
