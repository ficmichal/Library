using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.LibraryBranch;
using System;

namespace Library.Modules.Lending.Domain.Patrons.DomainEvents
{
    public class BookHoldCancelingFailed : IPatronEvent
    {
        public DateTime When { get; }
        public Guid PatronIdValue { get; }
        public Guid BookId { get; }
        public Guid LibraryBranchId { get; }

        public static BookHoldCancelingFailed HoldCancelingFailedNow(BookId bookId, LibraryBranchId libraryBranchId, PatronId patronId)
        {
            return new(DateTime.Now, bookId, libraryBranchId, patronId);
        }

        private BookHoldCancelingFailed(DateTime when, BookId bookId, LibraryBranchId libraryBranchId, PatronId patronId)
        {
            When = when;
            BookId = bookId.Id;
            LibraryBranchId = libraryBranchId.Id;
            PatronIdValue = patronId.Id;
        }
    }
}
