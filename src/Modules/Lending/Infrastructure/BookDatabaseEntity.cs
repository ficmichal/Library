using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Books.Types;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Domain.Patrons;
using System;

namespace Library.Modules.Lending.Infrastructure
{
    public class BookDatabaseEntity
    {
        public int Id { get; set; }

        public Guid BookId { get; set; }
        
        public BookType BookType { get; set; }
        
        public BookState BookState { get; set; }
        
        public Guid? AvailableAtBranch { get; set; }
        
        public Guid? OnHoldAtBranch { get; set; }
        
        public Guid? OnHoldByPatron{ get; set; }
        
        public DateTime? OnHoldTill { get; set; }

        public int Version { get; set; }

        public IBook ToDomainModel()
        {
            return BookState switch
            {
                BookState.Available => ToAvailableBook(),
                BookState.OnHold => ToOnHoldBook(),
                _ => null
            };
        }

        private AvailableBook ToAvailableBook()
        {
            return new(new BookId(BookId), BookType, BuildingBlocks.Domain.Version.Of(Version), new LibraryBranchId(AvailableAtBranch.GetValueOrDefault()));
        }

        private BookOnHold ToOnHoldBook()
        {
            return new(new BookId(BookId), BookType, new LibraryBranchId(OnHoldAtBranch.GetValueOrDefault()),
                new PatronId(OnHoldByPatron.GetValueOrDefault()), OnHoldTill, BuildingBlocks.Domain.Version.Of(Version));
        }
    }

    public enum BookState
    {
        Available,
        OnHold
    }
}
