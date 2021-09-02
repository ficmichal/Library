using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.Hold;
using System;

namespace Library.Modules.Lending.Application.Patrons.Hold
{
    public record PlaceOnHoldCommand
    {
        public DateTime Timestamp { get; }
        public PatronId PatronId { get; }
        public LibraryBranchId BranchId { get; }
        public BookId BookId { get; }
        private readonly int? _noOfDays;

        public static PlaceOnHoldCommand CloseEnded(PatronId patronId, LibraryBranchId libraryBranchId, BookId bookId,
            int forDays)
        {
            return new(DateTime.Now, patronId, libraryBranchId, bookId, forDays);
        }

        public static PlaceOnHoldCommand OpenEnded(PatronId patronId, LibraryBranchId libraryBranchId, BookId bookId)
        {
            return new(DateTime.Now, patronId, libraryBranchId, bookId, null);
        }

        public HoldDuration GetHoldDuration()
        {
            return _noOfDays.HasValue
                ? HoldDuration.CloseEnded(NumberOfDays.Of(_noOfDays.Value))
                : HoldDuration.OpenEnded();
        }

        private PlaceOnHoldCommand(DateTime timestamp, PatronId patronId, LibraryBranchId libraryBranchId, BookId bookId, int? noOfDays)
        {
            Timestamp = timestamp;
            PatronId = patronId;
            BranchId = libraryBranchId;
            BookId = bookId;
            _noOfDays = noOfDays;
        }
    }
}
