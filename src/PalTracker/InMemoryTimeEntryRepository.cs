using System.Collections.Generic;
using System.Linq;

namespace PalTracker
{
    public class InMemoryTimeEntryRepository : ITimeEntryRepository
    {
        private readonly IDictionary<long, TimeEntry> _timeEntries = new Dictionary<long, TimeEntry>();

        public TimeEntry Create(TimeEntry timeEntry)
        {
            var id = _timeEntries.Count + 1;

            timeEntry.Id = id;

            _timeEntries.Add(id, timeEntry);

            return timeEntry;
        }

        public TimeEntry Find(long id) => _timeEntries[id];

        public bool Contains(long id) => _timeEntries.ContainsKey(id);

        public IEnumerable<TimeEntry> List() => _timeEntries.Values.ToList();

        public TimeEntry Update(long id, TimeEntry timeEntry)
        {
            timeEntry.Id = id;

            _timeEntries[id] = timeEntry;

            return timeEntry;
        }

        public void Delete(long id)
        {
            _timeEntries.Remove(id);
        }
    }
}