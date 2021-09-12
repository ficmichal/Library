using System.Threading;
using System.Threading.Tasks;
using Library.BuildingBlocks.Infrastructure.Data;
using Library.BuildingBlocks.Infrastructure.Events;
using Library.BuildingBlocks.Infrastructure.Events.Modules;
using Library.Modules.Lending.Application.Books.EventListeners;
using Library.Modules.Lending.Application.Patrons.Hold;
using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Books.Types;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using Library.Modules.Lending.Domain.Patrons.Hold;
using Library.Modules.Lending.Infrastructure.Books;
using Library.Modules.Lending.Infrastructure.Patrons;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Books;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Data;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Patrons;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Probing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;
using static Library.Modules.Lending.Domain.Patrons.DomainEvents.PatronCreated;

namespace Library.Modules.Lending.IntegrationTests.Books
{
    public class DuplicateHoldFoundIT
    {
        private static readonly PatronId PatronId = PatronFixture.AnyPatronId;
        private static readonly PatronId AnotherPatronId = PatronFixture.AnyPatronId;

        private static readonly AvailableBook Book = BookFixture.CirculatingBook();
        private const string ConnectionString = DbFixture.ConnectionString;

        private readonly IBookRepository _bookRepo = new BookDatabaseRepository(new SqlConnectionFactory(ConnectionString));

        private IPatronRepository _patronRepo => serviceProvider.GetService<IPatronRepository>();
        private readonly ServiceProvider serviceProvider;
        private readonly BookPlacedOnHoldListener _bookPlacedOnHoldListener;

        public DuplicateHoldFoundIT()
        {
            _bookPlacedOnHoldListener = new BookPlacedOnHoldListener(null, null);
            var services = new ServiceCollection();
            services.AddEvents();
            services.AddEventDispatching();
            services.AddModuleRequests();
            services.AddTransient(_ => _bookRepo);
            services.AddTransient<ICancelingHold, CancelingHold>();
            services.AddDbContext<PatronsDbContext>(x => x.UseSqlServer(ConnectionString), ServiceLifetime.Transient);
            services.AddSingleton<IPatronRepository, PatronsDatabaseRepository>();
            serviceProvider = services.BuildServiceProvider();

            var eventDispatcher = serviceProvider.GetService<IHostedService>();
            eventDispatcher.StartAsync(CancellationToken.None);
        }

        [Fact]
        public async Task should_react_to_compensation_event_which_is_duplicate_hold_on_book_found()
        {
            // Given
            await _bookRepo.Save(Book);
            await _patronRepo.Publish(PatronCreated(PatronId));
            await _patronRepo.Publish(PatronCreated(AnotherPatronId));

            // When
            await _patronRepo.Publish(PlacedOnHold(Book, PatronId));
            await _patronRepo.Publish(PlacedOnHold(Book, AnotherPatronId));

            // Then
            await PatronShouldBeFoundInDatabaseWithZeroBookOnHold(2000);
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

        private async Task PatronShouldBeFoundInDatabaseWithZeroBookOnHold(int timeout)
        {
            await new Poller(timeout).CheckAsync(new PatronShouldBeFoundInDatabaseWithZeroBookOnHoldProbe(AnotherPatronId, _patronRepo));
        }

        public class PatronShouldBeFoundInDatabaseWithZeroBookOnHoldProbe : IProbe
        {
            private readonly PatronId _patronId;
            private readonly IPatronRepository _patronRepository;
            private int _numberOfHolds;

            public PatronShouldBeFoundInDatabaseWithZeroBookOnHoldProbe(PatronId patronId, IPatronRepository patronRepository)
            {
                _patronId = patronId;
                _patronRepository = patronRepository;
            }

            public async Task SampleAsync()
            {
                var patron = await _patronRepository.FindBy(_patronId);
                _numberOfHolds = patron.NumberOfHolds();
            }

            public bool IsSatisfied()
            {
                return _numberOfHolds == 0;
            }

            public string DescribeFailureTo()
            {
                return "When Patron tried unsuccessfully hold the book" +
                    "(because other patron already hold that book)" +
                    " it should not have that book in his BookOnHold.";
            }
        }
    }
}
