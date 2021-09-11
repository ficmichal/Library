using System.Threading.Tasks;

namespace Library.Modules.Lending.UnitTests.Shared.Fixtures.Probing
{
    public class Poller
    {
        private readonly int _timeoutMillis;

        private readonly int _pollDelayMillis;

        public Poller(int timeoutMillis)
        {
            _timeoutMillis = timeoutMillis;
            _pollDelayMillis = 1000;
        }

        public async Task CheckAsync(IProbe probe)
        {
            var timeout = new Timeout(_timeoutMillis);
            await probe.SampleAsync();
            while (!probe.IsSatisfied())
            {
                if (timeout.HasTimedOut())
                {
                    throw new AssertErrorException(DescribeFailureOf(probe));
                }

                await Task.Delay(_pollDelayMillis);
                await probe.SampleAsync();
            }
        }

        private static string DescribeFailureOf(IProbe probe)
        {
            return probe.DescribeFailureTo();
        }
    }
}
