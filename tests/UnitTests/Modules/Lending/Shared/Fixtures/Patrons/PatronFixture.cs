using Library.Modules.Lending.Domain.Books.Types;
using Library.Modules.Lending.Domain.Patrons;
using Library.Modules.Lending.Domain.Patrons.Hold;
using Library.Modules.Lending.Domain.Patrons.Policies;
using Library.Modules.Lending.UnitTests.Shared.Fixtures.Books;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Modules.Lending.UnitTests.Shared.Fixtures.Patrons
{
    public class PatronFixture
    {
        public static PatronId AnyPatronId => new(Guid.NewGuid());

        public static Patron RegularPatron()
        {
            return RegularPatron(AnyPatronId);
        }

        public static Patron RegularPatron(PatronId patronId)
        {
            return new Patron(
                PatronInformation(patronId, PatronType.Regular),
                NoHolds(),
                Enumerable.Empty<IPlacingOnHoldPolicy>().ToList());
        }

        public static Patron RegularPatronWithPolicy(IPlacingOnHoldPolicy placingOnHoldPolicy)
        {
            return PatronWithPolicy(AnyPatronId, PatronType.Regular, placingOnHoldPolicy);
        }

        public static Patron ResearcherPatronWithPolicy(IPlacingOnHoldPolicy placingOnHoldPolicy)
        {
            return PatronWithPolicy(AnyPatronId, PatronType.Researcher, placingOnHoldPolicy);
        }

        public static Patron RegularPatronWithHolds(int numberOfHolds)
        {
            var patronId = AnyPatronId;

            return new Patron(
                PatronInformation(patronId, PatronType.Regular),
                BooksOnHold(numberOfHolds),
                new List<IPlacingOnHoldPolicy> {new RegularPatronMaximumNumberOfHoldsPolicy()});
        }

        public static Patron RegularPatronWithHold(PatronId patronId, BookOnHold bookOnHold)
        {
            return RegularPatronWith(patronId, new Hold(bookOnHold.Id, bookOnHold.HoldPlacedAt));
        }

        private static Patron PatronWithPolicy(PatronId patronId, PatronType patronType,
            IPlacingOnHoldPolicy placingOnHoldPolicy)
        {
            return new(new PatronInformation(patronId, patronType), NoHolds(),
                new List<IPlacingOnHoldPolicy> {placingOnHoldPolicy});
        }

        private static Patron RegularPatronWith(PatronId patronId, Hold hold)
        {
            var patronHolds = new PatronHolds(new HashSet<Hold> { hold });
            return new Patron(
                    new PatronInformation(patronId, PatronType.Regular),
                    patronHolds,
                    AllCurrentPolicies.Get());
        }

        private static PatronInformation PatronInformation(PatronId patronId, PatronType patronType)
        {
            return new(patronId, patronType);
        }

        private static PatronHolds NoHolds()
        {
            return new(Enumerable.Empty<Hold>().ToHashSet());
        }

        private static PatronHolds BooksOnHold(int numberOfHolds)
        {
            return new(Enumerable
                    .Range(1, numberOfHolds)
                    .Select(_ => new Hold(BookFixture.AnyBookId, BookFixture.AnyBranchId))
                    .ToHashSet());
        }
    }
}
