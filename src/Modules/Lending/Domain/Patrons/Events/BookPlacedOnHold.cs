using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Domain.Patrons.Hold;
using System;

namespace Library.Modules.Lending.Domain.Patrons.Events
{
    public class BookPlacedOnHold : IPatronEvent
    {
        public Guid EventId => new();

        public DateTime When { get; }

        public Guid PatronIdValue { get; }

        public Guid BookId { get; }

        public BookType BookType { get; }

        public Guid LibraryBranchId { get; }

        public DateTime HoldFrom { get; }

        public DateTime? HoldTill { get; }

        public static BookPlacedOnHold BookPlacedOnHoldNow(PatronId patronId, BookId bookId, BookType bookType,
            LibraryBranchId libraryBranchId, HoldDuration holdDuration)
        {
            return new(patronId, bookId, bookType, libraryBranchId, holdDuration);
        }

        private BookPlacedOnHold(PatronId patronId, BookId bookId, BookType bookType, LibraryBranchId libraryBranchId, HoldDuration holdDuration)
        {
            When = DateTime.Now;
            PatronIdValue = patronId.Id;
            BookId = bookId.Id;
            BookType = bookType;
            LibraryBranchId = libraryBranchId.Id;
            HoldFrom = holdDuration.From;
            HoldTill = holdDuration.To;
        }
    }
}
