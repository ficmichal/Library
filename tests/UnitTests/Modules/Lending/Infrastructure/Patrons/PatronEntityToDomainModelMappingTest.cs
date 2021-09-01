using FluentAssertions;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Infrastructure.Patrons;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Library.Modules.Lending.Infrastructure.UnitTests.Patrons
{
    public class PatronEntityToDomainModelMappingTest
    {
        [Fact]
        public void ShouldMapPatronHolds()
        {
            // Given
            var libraryBranchId = BookFixture.AnyBranchId;
            var anotherBranchId = BookFixture.AnyBranchId;
            var patronId = BookFixture.AnyPatronId;
            var bookId = BookFixture.AnyBookId;
            var anotherBookId = BookFixture.AnyBookId;
            var anyDate = DateTime.Now;

            var entity = PatronEntity(patronId, PatronType.Regular, new List<HoldDatabaseEntity>
            {
                new(bookId.Id, patronId.Id, libraryBranchId.Id, anyDate),
                new(anotherBookId.Id, patronId.Id, anotherBranchId.Id, anyDate),
            });

            // When
            var patron = DomainModelMapper.Map(entity);

            // Then
            patron.PatronHolds.Count.Should().Be(2);
        }

        private PatronDatabaseEntity PatronEntity(PatronId patronId, PatronType type, List<HoldDatabaseEntity> holds)
        {
            return new()
            {
                PatronId = patronId.Id,
                PatronType = type,
                BooksOnHold = holds
            };
        }
    }
}
