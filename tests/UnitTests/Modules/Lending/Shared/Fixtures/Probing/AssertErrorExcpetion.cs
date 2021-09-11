using System;

namespace Library.Modules.Lending.UnitTests.Shared.Fixtures.Probing
{
    public class AssertErrorException : Exception
    {
        public AssertErrorException(string message)
            : base(message)
        {
        }
    }
}
