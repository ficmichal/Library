using Library.BuildingBlocks.Domain;
using Library.BuildingBlocks.Domain.Policies;
using Library.Modules.Lending.Domain.Books.Types;
using Library.Modules.Lending.Domain.Patrons.DomainEvents;
using Library.Modules.Lending.Domain.Patrons.Hold;
using Library.Modules.Lending.Domain.Patrons.Policies;
using System.Collections.Generic;
using System.Linq;
using static Library.Modules.Lending.Domain.Patrons.DomainEvents.BookHoldFailed;
using static Library.Modules.Lending.Domain.Patrons.DomainEvents.BookPlacedOnHold;
using static Library.Modules.Lending.Domain.Patrons.DomainEvents.BookPlacedOnHoldEvents;

namespace Library.Modules.Lending.Domain.Patrons
{
    public class Patron : ValueObject
    {
        public Patron(PatronInformation patronInformation, PatronHolds patronHolds, List<IPlacingOnHoldPolicy> placingOnHoldPolicies)
        {
            PatronInformation = patronInformation;
            PatronHolds = patronHolds;
            _placingOnHoldPolicies = placingOnHoldPolicies;
        }

        public PatronInformation PatronInformation { get; }

        public PatronHolds PatronHolds { get; }

        private readonly List<IPlacingOnHoldPolicy> _placingOnHoldPolicies;

        public IPatronEvent PlaceOnHold(AvailableBook book)
        {
            return PlaceOnHold(book, HoldDuration.OpenEnded());
        }

        public IPatronEvent PlaceOnHold(AvailableBook book, HoldDuration holdDuration)
        {
            var rejection = PatronCanHold(book, holdDuration);
            if (rejection is { })
            {
                return BookHoldFailedNow(rejection, book.Id, book.LibraryBranchId, PatronInformation);
            }

            var bookPlacedOnHold = BookPlacedOnHoldNow(PatronInformation.PatronId, book.Id, book.Type,
                book.LibraryBranchId, holdDuration);

            if (PatronHolds.MaximumHoldsAfterHolding())
            {
                return Events(bookPlacedOnHold,
                    MaximumNumberOhHoldsReached.Now(PatronInformation, PatronHolds.MaximumNumberOfHolds));
            }

            return Events(bookPlacedOnHold);
        }

        public int NumberOfHolds() => PatronHolds.Count;

        private Rejection PatronCanHold(AvailableBook aBook, HoldDuration holdDuration)
        {
            return _placingOnHoldPolicies
                .Select(x => x.Check(aBook, this, holdDuration))
                .OfType<Rejection>()
                .FirstOrDefault();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PatronInformation;
        }
    }
}
