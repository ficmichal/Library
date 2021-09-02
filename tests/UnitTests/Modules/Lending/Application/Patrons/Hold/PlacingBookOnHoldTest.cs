using FluentAssertions;
using Library.BuildingBlocks.Domain.Commands;
using Library.Modules.Lending.Application.Patrons.Hold;
using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Patrons;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Threading.Tasks;
using Xunit;
using static Library.Modules.Lending.UnitTests.Shared.Fixtures.Books.BookFixture;

namespace Library.Modules.Lending.Application.UnitTests.Patrons.Hold
{
    public class PlacingBookOnHoldTest
    {
        private static IBookRepository WillFindBook()
        {
            var substitute = Substitute
                .For<IBookRepository>();

            substitute.FindBy(Arg.Any<BookId>()).Returns(Task.FromResult(CirculatingBook() as IBook));
            return substitute;
        }

        private static IBookRepository WillNotFindBook()
        {
            var substitute = Substitute
                .For<IBookRepository>();

            substitute.FindBy(Arg.Any<BookId>()).Returns(Task.FromResult<IBook>(null));
            return substitute;
        }

        private readonly IPatronRepository _repository = Substitute.For<IPatronRepository>();

        [Fact]
        public async Task should_successfully_place_on_hold_book_if_patron_and_book_exist()
        {
            var holding = new PlacingOnHold(WillFindBook(), _repository);
            var patronId = PersistedRegularPatron();

            var result = await holding.PlaceOnHold(ForThreeDays(patronId));

            result.Should().Be(Result.Success);
        }

        [Fact]
        public async Task should_reject_placing_on_hold_book_if_one_of_the_domain_rules_is_broken()
        {
            var holding = new PlacingOnHold(WillFindBook(), _repository);
            var patronId = PersistedRegularPatronWithManyHolds();

            var result = await holding.PlaceOnHold(ForThreeDays(patronId));

            result.Should().Be(Result.Rejection);
        }

        [Fact]
        public async Task should_fail_if_patron_does_not_exist()
        {
            var holding = new PlacingOnHold(WillFindBook(), _repository);
            var patronId = UnknownPatron();

            Func<Task<Result>> placingOnHold = async () => await holding.PlaceOnHold(ForThreeDays(patronId));

            await placingOnHold.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task should_fail_if_book_does_not_exist()
        {
            var holding = new PlacingOnHold(WillNotFindBook(), _repository);
            var patronId = PersistedRegularPatron();

            Func<Task<Result>> placingOnHold = async () => await holding.PlaceOnHold(ForThreeDays(patronId));

            await placingOnHold.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task should_fail_if_saving_patron_fails()
        {
            var holding = new PlacingOnHold(WillFindBook(), _repository);
            var patronId = PersistedRegularPatronThatFailsOnSaving();

            Func<Task<Result>> placingOnHold = async () => await holding.PlaceOnHold(ForThreeDays(patronId));

            await placingOnHold.Should().ThrowAsync<Exception>();
        }

        private static PlaceOnHoldCommand ForThreeDays(PatronId patronId)
        {
            return PlaceOnHoldCommand.CloseEnded(patronId, AnyBranchId, AnyBookId, 4);
        }

        private PatronId PersistedRegularPatron()
        {
            var patronId = PatronFixture.AnyPatronId;
            var patron = PatronFixture.RegularPatron(patronId);

            _repository.FindBy(patronId).Returns(Task.FromResult(patron));
            _repository.Publish(Arg.Any<IPatronEvent>()).Returns(Task.FromResult(patron));

            return patronId;
        }

        private PatronId PersistedRegularPatronWithManyHolds()
        {
            var patronId = PatronFixture.AnyPatronId;
            var patron = PatronFixture.RegularPatronWithHolds(10);

            _repository.FindBy(patronId).Returns(Task.FromResult(patron));
            _repository.Publish(Arg.Any<IPatronEvent>()).Returns(Task.FromResult(patron));

            return patronId;
        }

        private PatronId PersistedRegularPatronThatFailsOnSaving()
        {
            var patronId = PatronFixture.AnyPatronId;
            var patron = PatronFixture.RegularPatron(patronId);

            _repository.FindBy(patronId).Returns(Task.FromResult(patron));
            _repository.Publish(Arg.Any<IPatronEvent>()).Throws(new Exception());

            return patronId;
        }

        private static PatronId UnknownPatron()
        {
            return AnyPatronId;
        }
    }
}
