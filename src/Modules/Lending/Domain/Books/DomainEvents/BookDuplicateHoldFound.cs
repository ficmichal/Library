using Library.BuildingBlocks.Domain.Events;
using System;

namespace Library.Modules.Lending.Domain.Books.DomainEvents
{
    public class BookDuplicateHoldFound : IDomainEvent
    {
        public Guid EventId => Guid.NewGuid();
        public Guid AggregateId => BookId;
        public DateTime When { get; }
        public Guid FirstPatronId { get; }
        public Guid SecondPatronId { get; }
        public Guid LibraryBranchId { get; }
        public Guid BookId { get; }

        public static BookDuplicateHoldFound Create(DateTime @when, Guid firstPatronId, Guid secondPatronId, Guid libraryBranchId, Guid bookId)
        {
            return new BookDuplicateHoldFound(@when, firstPatronId, secondPatronId, libraryBranchId, bookId);
        }

        public static BookDuplicateHoldFound Now(Guid firstPatronId, Guid secondPatronId, Guid libraryBranchId, Guid bookId)
        {
            var now = DateTime.Now;
            return new BookDuplicateHoldFound(now, firstPatronId, secondPatronId, libraryBranchId, bookId);
        }

        private BookDuplicateHoldFound(DateTime @when, Guid firstPatronId, Guid secondPatronId, Guid libraryBranchId, Guid bookId)
        {
            When = when;
            FirstPatronId = firstPatronId;
            SecondPatronId = secondPatronId;
            LibraryBranchId = libraryBranchId;
            BookId = bookId;
        }
    }
}
