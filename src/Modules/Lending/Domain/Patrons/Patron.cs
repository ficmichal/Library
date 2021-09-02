using Library.BuildingBlocks.Domain;
using Library.Modules.Lending.Domain.Books.Types;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using Library.Modules.Lending.Domain.Patrons.Hold;
using System.Collections.Generic;
using static Library.Modules.Lending.Domain.Patrons.DomainEvents.BookPlacedOnHold;
using static Library.Modules.Lending.Domain.Patrons.DomainEvents.BookPlacedOnHoldEvents;

namespace Library.Modules.Lending.Domain.Patrons
{
    public class Patron : ValueObject
    {
        public Patron(PatronInformation patronInformation, PatronHolds patronHolds)
        {
            PatronInformation = patronInformation;
            PatronHolds = patronHolds;
        }

        public PatronInformation PatronInformation { get; }

        public PatronHolds PatronHolds { get; }

        public IPatronEvent PlaceOnHold(AvailableBook book)
        {
            return PlaceOnHold(book, HoldDuration.OpenEnded());
        }

        public IPatronEvent PlaceOnHold(AvailableBook book, HoldDuration holdDuration)
        {
            var bookPlacedOnHold = BookPlacedOnHoldNow(PatronInformation.PatronId, book.Id, book.Type,
                book.LibraryBranchId, holdDuration);

            return Events(bookPlacedOnHold);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PatronInformation;
        }
    }
}
