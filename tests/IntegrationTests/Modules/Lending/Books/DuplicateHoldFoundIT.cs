using FluentAssertions;
using Library.BuildingBlocks.Domain.Events;
using Library.BuildingBlocks.Infrastructure.Data;
using Library.BuildingBlocks.Infrastructure.Events;
using Library.BuildingBlocks.Infrastructure.Events.Dispatchers;
using Library.BuildingBlocks.Infrastructure.Events.Modules;
using Library.Modules.Lending.Application.Books.EventListeners;
using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Books.Types;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using Library.Modules.Lending.Domain.Patrons.Hold;
using Library.Modules.Lending.Infrastructure.Books;
using Library.Modules.Lending.Infrastructure.Patrons;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Books;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Data;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.LibraryBranches;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Patrons;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Library.Modules.Lending.Domain.Patrons.DomainEvents.PatronCreated;

namespace Library.Modules.Lending.IntegrationTests.Books
{
    public class DuplicateHoldFoundIT
    {
        private static readonly PatronId PatronId = PatronFixture.AnyPatronId;
        private static readonly PatronId AnotherPatronId = PatronFixture.AnyPatronId;

        private static readonly LibraryBranchId LibraryBranchId = LibraryBranchFixture.AnyBranchId;

        private static readonly AvailableBook Book = BookFixture.CirculatingBook();
        private const string ConnectionString = DbFixture.ConnectionString;

        private readonly BookDatabaseRepository _bookRepo = new(new SqlConnectionFactory(ConnectionString));

        private readonly PatronsDatabaseRepository _patronRepo;

        private readonly BookPlacedOnHoldListener _bookPlacedOnHoldListener;

        public DuplicateHoldFoundIT()
        {
            _bookPlacedOnHoldListener = new BookPlacedOnHoldListener(null, null);
            var services = new ServiceCollection();
            services.AddEvents();
            services.AddEventDispatching();
            services.AddModuleRequests();
            services.AddTransient<IBookRepository>(_ => _bookRepo);
            var serviceProvider = services.BuildServiceProvider();

            var eventDispatcher = serviceProvider.GetService<IHostedService>();
            eventDispatcher.StartAsync(CancellationToken.None);
            
            _patronRepo = new(new PatronsDbContext(ConnectionString), serviceProvider.GetService<IDomainEvents>());
        }

        [Fact]
        public async Task When()
        {
            // Given
            await _bookRepo.Save(Book);
            await _patronRepo.Publish(PatronCreated(PatronId));
            await _patronRepo.Publish(PatronCreated(AnotherPatronId));

            // When
            await _patronRepo.Publish(PlacedOnHold(Book, PatronId));
            await _patronRepo.Publish(PlacedOnHold(Book, AnotherPatronId));

            // Then
            await PatronShouldBeFoundInDatabaseWithZeroBookOnHold(AnotherPatronId);
        }

        BookPlacedOnHold PlacedOnHold(AvailableBook book, PatronId patronId)
        {
            return BookPlacedOnHold.BookPlacedOnHoldNow(
                patronId,
                book.Id,
                book.Type,
                book.LibraryBranchId,
                HoldDuration.CloseEnded(5));
        }
        private PatronCreated PatronCreated(PatronId patronId)
        {
            return Now(patronId, PatronType.Regular);
        }

        private async Task PatronShouldBeFoundInDatabaseWithZeroBookOnHold(PatronId patronId)
        {
            var patron = await LoadPersistedPatron(patronId);
            patron.NumberOfHolds().Should().Be(0);
        }

        private async Task<Patron> LoadPersistedPatron(PatronId patronId)
        {
            var loaded = await _patronRepo.FindBy(patronId);

            return loaded;
        }
    }
}
