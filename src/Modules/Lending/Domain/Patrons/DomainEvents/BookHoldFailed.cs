using Library.BuildingBlocks.Domain.Policies;
using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.LibraryBranch;
using System;
using System.Text.Json.Serialization;

namespace Library.Modules.Lending.Domain.Patrons.DomainEvents
{
    public class BookHoldFailed : IPatronEvent
    {
        public Guid EventId => new();
        public string Reason { get; }
        public DateTime When { get; }
        public Guid PatronIdValue { get; }
        public Guid BookId { get; }
        public Guid LibraryBranchId { get; }

        public static BookHoldFailed BookHoldFailedNow(Rejection rejection, BookId bookId,
            LibraryBranchId libraryBranchId, PatronInformation patronInformation)
        {
            return new(
                rejection.Reason.Value,
                patronInformation.PatronId.Id,
                bookId.Id,
                libraryBranchId.Id);
        }

        [JsonConstructor]
        private BookHoldFailed(string reason, Guid patronId, Guid bookId, Guid libraryBranchId)
        {
            Reason = reason;
            When = DateTime.Now;
            PatronIdValue = patronId;
            BookId = bookId;
            LibraryBranchId = libraryBranchId;
        }
    }
}
