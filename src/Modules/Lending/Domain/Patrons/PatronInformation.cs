using Library.BuildingBlocks.Domain;
using System.Collections.Generic;

namespace Library.Modules.Lending.Domain.Patrons
{
    public class PatronInformation : ValueObject
    {
        public PatronInformation(PatronId patronId, PatronType patronType)
        {
            PatronId = patronId;
            PatronType = patronType;
        }

        public PatronId PatronId { get; }

        public PatronType PatronType { get; }


        public bool IsRegular()
        {
            return PatronType == PatronType.Regular;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PatronId;
            yield return PatronType;
        }
    }
}
