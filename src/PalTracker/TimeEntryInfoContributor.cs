using Steeltoe.Management.Endpoint.Info;

namespace PalTracker
{
    public class TimeEntryInfoContributor : IInfoContributor
    {
        private readonly IOperationCounter<TimeEntry> _operationCounter;

        public TimeEntryInfoContributor(IOperationCounter<TimeEntry> operationCounter)
        {
            _operationCounter = operationCounter;
        }

        public void Contribute(IInfoBuilder builder)
        {
            builder.WithInfo(
                _operationCounter.Name,
                _operationCounter.GetCounts
            );
        }
    }
}