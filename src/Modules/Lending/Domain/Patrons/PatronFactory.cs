namespace Library.Modules.Lending.Domain.Patrons
{
    public class PatronFactory
    {
        public static Patron Create(PatronType patronType, PatronId patronId)
        {
            return new(new PatronInformation(patronId, patronType));
        }
    }
}
