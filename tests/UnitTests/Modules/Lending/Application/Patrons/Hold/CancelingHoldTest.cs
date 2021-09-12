using System;
using System.Threading.Tasks;
using FluentAssertions;
using Library.BuildingBlocks.Domain.Commands;
using Library.Modules.Lending.Application.Patrons.Hold;
using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Books.Types;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Books;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Patrons;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;
using static Library.Modules.Lending.UnitTests.Shared.Fixtures.Books.BookFixture;

namespace Library.Modules.Lending.Application.UnitTests.Patrons.Hold
{
    public class CancelingHoldTest
    {
        private readonly IPatronRepository _repository = Substitute.For<IPatronRepository>();
        private readonly PatronId _patronId = PatronFixture.AnyPatronId;
        private readonly static BookOnHold _bookOnHold = BookOnHold();

        [Fact]
        public async Task should_successfully_cancel_hold_if_book_was_placed_on_hold_by_patron_and_patron_and_book_exist()
        {
            // Given
            var canceling = new CancelingHold(WillFindBook(), _repository);
            PersistedRegularPatronWithBookOnHold();

            // When
            var result = await canceling.CancelHold(Command());

            // Then
            result.Should().Be(BuildingBlocks.Domain.Commands.Result.Success);
        }

        [Fact]
        public async Task should_reject_placing_on_hold_book_if_one_of_the_domain_rules_is_broken()
        {
            // Given
            var canceling = new CancelingHold(WillFindBook(), _repository);
            PersistedRegularPatronWithManyHolds();

            // When
            var result = await canceling.CancelHold(Command());

            // Then
            result.Should().Be(BuildingBlocks.Domain.Commands.Result.Rejection);
        }

        [Fact]
        public async Task should_fail_if_patron_does_not_exists()
        {
            // Given
            var canceling = new CancelingHold(WillFindBook(), _repository);
            UnknownPatron();

            // When
            Func<Task<Result>> cancelingHold = async () => await canceling.CancelHold(Command());

            // Then
            await cancelingHold.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task should_fail_if_book_does_not_exists()
        {
            // Given
            var canceling = new CancelingHold(WillNotFindBook(), _repository);
            PersistedRegularPatronWithBookOnHold();

            // When
            Func<Task<Result>> cancelingHold = async () => await canceling.CancelHold(Command());

            // Then
            await cancelingHold.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task should_fail_if_saving_patron_fails()
        {
            // Given
            var canceling = new CancelingHold(WillFindBook(), _repository);
            PersistedRegularPatronThatFailsOnSaving();

            // When
            Func<Task<Result>> cancelingHold = async () => await canceling.CancelHold(Command());

            // Then
            await cancelingHold.Should().ThrowAsync<Exception>();
        }

        private static IBookRepository WillFindBook()
        {
            var substitute = Substitute
                .For<IBookRepository>();

            substitute.FindBy(Arg.Any<BookId>()).Returns(Task.FromResult(_bookOnHold as IBook));
            return substitute;
        }

        private static IBookRepository WillNotFindBook()
        {
            var substitute = Substitute
                .For<IBookRepository>();

            substitute.FindBy(Arg.Any<BookId>()).Returns(Task.FromResult<IBook>(null));
            return substitute;
        }

        private CancelHoldCommand Command()
        {
            return new CancelHoldCommand(DateTime.Now, _patronId, BookFixture.AnyBookId);
        }

        private PatronId PersistedRegularPatronWithBookOnHold()
        {
            var patron = PatronFixture.RegularPatronWithHold(_patronId, _bookOnHold);

            _repository.FindBy(_patronId).Returns(Task.FromResult(patron));
            _repository.Publish(Arg.Any<IPatronEvent>()).Returns(Task.FromResult(patron));

            return _patronId;
        }

        private PatronId PersistedRegularPatronWithManyHolds()
        {
            var patron = PatronFixture.RegularPatronWithHolds(10);

            _repository.FindBy(_patronId).Returns(Task.FromResult(patron));
            _repository.Publish(Arg.Any<IPatronEvent>()).Returns(Task.FromResult(patron));

            return _patronId;
        }

        private PatronId PersistedRegularPatronThatFailsOnSaving()
        {
            var patron = PatronFixture.RegularPatronWithHold(_patronId, _bookOnHold);

            _repository.FindBy(_patronId).Returns(Task.FromResult(patron));
            _repository.Publish(Arg.Any<IPatronEvent>()).Throws(new Exception());

            return _patronId;
        }

        private static PatronId UnknownPatron()
        {
            return AnyPatronId;
        }
    }
}
