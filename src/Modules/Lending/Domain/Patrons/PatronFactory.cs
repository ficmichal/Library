using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.LibraryBranch;
using System.Collections.Generic;
using System.Linq;

namespace Library.Modules.Lending.Domain.Patrons
{
    public class PatronFactory
    {
        public static Patron Create(PatronType patronType, PatronId patronId, ISet<(BookId, LibraryBranchId)> patronHolds)
        {
            return new(
                new PatronInformation(patronId, patronType),
                new PatronHolds(patronHolds
                    .Select(hold => new Hold.Hold(hold.Item1, hold.Item2))
                    .ToHashSet()));
        }
    }
}
