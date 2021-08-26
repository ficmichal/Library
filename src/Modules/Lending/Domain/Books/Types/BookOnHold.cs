using Library.BuildingBlocks.Domain;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Domain.Patrons;
using System;
using System.Collections.Generic;
using Version = Library.BuildingBlocks.Domain.Version;

namespace Library.Modules.Lending.Domain.Books.Types
{
    public class BookOnHold : ValueObject, IBook
    {
        public BookOnHold(BookId id, BookType type, LibraryBranchId holdPlacedAt, PatronId byPatron, DateTime holdTill, Version version)
        {
            Id = id;
            Type = type;
            Information = new BookInformation(id, type);
            HoldPlacedAt = holdPlacedAt;
            ByPatron = byPatron;
            HoldTill = holdTill;
            Version = version;
        }

        public BookId Id { get; }

        public BookType Type { get; }

        public BookInformation Information { get; }

        public LibraryBranchId HoldPlacedAt { get; }

        public PatronId ByPatron { get; }

        public DateTime HoldTill { get; }

        public Version Version { get; }

        public bool By(PatronId patronId)
        {
            return ByPatron.Equals(patronId);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Information;
        }
    }
}
