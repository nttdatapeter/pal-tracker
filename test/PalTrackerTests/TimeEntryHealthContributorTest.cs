using System.Collections.Generic;
using System.Linq;
using Moq;
using PalTracker;
using Xunit;
using static PalTracker.TimeEntryHealthContributor;
using static Steeltoe.Common.HealthChecks.HealthStatus;

namespace PalTrackerTests
{
    public class TimeEntryHealthContributorTest
    {
        private readonly TimeEntryHealthContributor _contributor;
        private readonly Mock<ITimeEntryRepository> _repository;

        public TimeEntryHealthContributorTest()
        {
            _repository = new Mock<ITimeEntryRepository>();
            _contributor = new TimeEntryHealthContributor(_repository.Object);
        }

        [Fact]
        public void Health_StatusUp_WhenBelowThreshold()
        {
            const int timeEntryCount = MaxTimeEntries - 1;

            _repository.Setup(r => r.List())
                .Returns(MakeTimeEntries(timeEntryCount));

            Assert.Equal(UP, _contributor.Health().Status);
            Assert.Equal(MaxTimeEntries, _contributor.Health().Details["threshold"]);
            Assert.Equal(timeEntryCount, _contributor.Health().Details["count"]);
            Assert.Equal("UP", _contributor.Health().Details["status"]);
        }

        [Fact]
        public void Health_StatusDown_WhenAtThreshold()
        {
            const int timeEntryCount = MaxTimeEntries;

            _repository.Setup(r => r.List())
                .Returns(MakeTimeEntries(timeEntryCount));

            Assert.Equal(DOWN, _contributor.Health().Status);
            Assert.Equal(MaxTimeEntries, _contributor.Health().Details["threshold"]);
            Assert.Equal(timeEntryCount, _contributor.Health().Details["count"]);
            Assert.Equal("DOWN", _contributor.Health().Details["status"]);
        }

        [Fact]
        public void Health_StatusDown_WhenAboveThreshold()
        {
            const int timeEntryCount = MaxTimeEntries + 1;

            _repository.Setup(r => r.List())
                .Returns(MakeTimeEntries(timeEntryCount));

            Assert.Equal(DOWN, _contributor.Health().Status);
            Assert.Equal(MaxTimeEntries, _contributor.Health().Details["threshold"]);
            Assert.Equal(timeEntryCount, _contributor.Health().Details["count"]);
            Assert.Equal("DOWN", _contributor.Health().Details["status"]);
        }

        [Fact]
        public void Id()
        {
            Assert.Equal("timeEntry", _contributor.Id);
        }


        private static IEnumerable<TimeEntry> MakeTimeEntries(int count) =>
            Enumerable.Range(0, count).Select(x => new TimeEntry());
    }
}