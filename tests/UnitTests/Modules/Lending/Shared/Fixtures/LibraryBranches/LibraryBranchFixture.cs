using Library.Modules.Lending.Domain.LibraryBranch;
using System;

namespace Library.Modules.Lending.UnitTests.Shared.Fixtures.LibraryBranches
{
    public class LibraryBranchFixture
    {
        public static LibraryBranchId AnyBranchId => new(Guid.NewGuid());
    }
}
