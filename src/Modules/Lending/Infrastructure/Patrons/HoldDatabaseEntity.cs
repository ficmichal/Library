using System;

namespace Library.Modules.Lending.Infrastructure.Patrons
{
    public class HoldDatabaseEntity
    {
        public int Id { get; set; }

        public Guid PatronId { get; set; }

        public Guid BookId { get; set; }

        public Guid LibraryBranchId { get; set; }

        public int PatronDatabaseEntity { get; set; }

        public DateTime? Till { get; set; }

        public HoldDatabaseEntity(Guid bookId, Guid patronId, Guid libraryBranchId, DateTime? till)
        {
            BookId = bookId;
            PatronId = patronId;
            LibraryBranchId = libraryBranchId;
            Till = till;
        }

        public bool Is(Guid bookId, Guid patronId, Guid libraryBranchId)
        {
            return BookId.Equals(bookId) && PatronId.Equals(patronId) && LibraryBranchId.Equals(libraryBranchId);
        }
    }
}
