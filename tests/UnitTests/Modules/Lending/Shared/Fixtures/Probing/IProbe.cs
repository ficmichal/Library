using System.Threading.Tasks;

namespace Library.Modules.Lending.UnitTests.Shared.Fixtures.Probing
{
    public interface IProbe
    {
        bool IsSatisfied();

        Task SampleAsync();

        string DescribeFailureTo();
    }
}
