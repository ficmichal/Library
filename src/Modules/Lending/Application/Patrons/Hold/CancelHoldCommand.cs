using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Patrons;
using System;

namespace Library.Modules.Lending.Application.Patrons.Hold
{
    public class CancelHoldCommand
    {
        public CancelHoldCommand(DateTime timestamp, PatronId patronId, BookId bookId)
        {
            Timestamp = timestamp;
            PatronId = patronId;
            BookId = bookId;
        }

        public DateTime Timestamp { get; }
        public PatronId PatronId { get; }
        public BookId BookId { get; }
    }
}
