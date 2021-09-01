using Library.Modules.Lending.Domain.Patrons;
using System;

namespace Library.Modules.Lending.UnitTests.Shared.Fixtures.Patrons
{
    public class PatronFixture
    {
        public static PatronId AnyPatronId => new(Guid.NewGuid());
    }
}
