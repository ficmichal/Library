using Library.BuildingBlocks.Domain;
using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.LibraryBranch;
using System.Collections.Generic;

namespace Library.Modules.Lending.Domain.Patrons.Hold
{
    public class Hold : ValueObject
    {
        public Hold(BookId bookId, LibraryBranchId libraryBranchId)
        {
            BookId = bookId;
            LibraryBranchId = libraryBranchId;
        }

        public BookId BookId { get; }

        public LibraryBranchId LibraryBranchId { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return BookId;
            yield return LibraryBranchId;
        }
    }
}
