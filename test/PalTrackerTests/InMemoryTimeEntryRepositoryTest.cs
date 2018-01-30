using System;
using System.Linq;
using PalTracker;
using Xunit;

namespace PalTrackerTests
{
    public class InMemoryTimeEntryRepositoryTest
    {
        private readonly InMemoryTimeEntryRepository _repository;

        public InMemoryTimeEntryRepositoryTest()
        {
            _repository = new InMemoryTimeEntryRepository();
        }

        [Fact]
        public void Create()
        {
            var expected = new TimeEntry(1, 222, 333, new DateTime(2008, 08, 01, 12, 00, 01), 24);

            var created = _repository.Create(new TimeEntry(222, 333, new DateTime(2008, 08, 01, 12, 00, 01), 24));

            Assert.Equal(expected, created);
            Assert.Equal(expected, _repository.Find(1));
        }

        [Fact]
        public void Find()
        {
            _repository.Create(new TimeEntry(222, 333, new DateTime(2008, 08, 01, 12, 00, 01), 24));

            var found = _repository.Find(1);

            Assert.Equal(new TimeEntry(1, 222, 333, new DateTime(2008, 08, 01, 12, 00, 01), 24), found);
        }

        [Fact]
        public void Contains()
        {
            _repository.Create(new TimeEntry(222, 333, new DateTime(2008, 08, 01, 12, 00, 01), 24));

            _repository.Contains(1);

            Assert.True(_repository.Contains(1));
            Assert.False(_repository.Contains(2));
        }

        [Fact]
        public void List()
        {
            _repository.Create(new TimeEntry(222, 333, new DateTime(2008, 08, 01, 12, 00, 01), 24));
            _repository.Create(new TimeEntry(888, 777, new DateTime(2012, 09, 02, 11, 30, 00), 12));

            var found = _repository.List();

            Assert.Contains(new TimeEntry(2, 888, 777, new DateTime(2012, 09, 02, 11, 30, 00), 12), found);
            Assert.Contains(new TimeEntry(1, 222, 333, new DateTime(2008, 08, 01, 12, 00, 01), 24), found);
            Assert.Equal(2, found.Count());
        }

        [Fact]
        public void Update()
        {
            _repository.Create(new TimeEntry(222, 333, new DateTime(2008, 08, 01, 12, 00, 01), 24));

            _repository.Update(1, new TimeEntry(555, 666, new DateTime(2020, 08, 01, 01, 55, 10), 8));

            var entires = _repository.List();
            Assert.Contains(new TimeEntry(1, 555, 666, new DateTime(2020, 08, 01, 01, 55, 10), 8), entires);
            Assert.DoesNotContain(new TimeEntry(1, 222, 333, new DateTime(2008, 08, 01, 12, 00, 01), 24), entires);
        }

        [Fact]
        public void Delete()
        {
            _repository.Create(new TimeEntry(222, 333, new DateTime(2008, 08, 01, 12, 00, 01), 24));
            _repository.Create(new TimeEntry(888, 777, new DateTime(2012, 09, 02, 11, 30, 00), 12));

            _repository.Delete(1);

            var remaining = _repository.List();
            Assert.DoesNotContain(new TimeEntry(1, 222, 333, new DateTime(2008, 08, 01, 12, 00, 01), 24), remaining);
            Assert.Contains(new TimeEntry(2, 888, 777, new DateTime(2012, 09, 02, 11, 30, 00), 12), remaining);
            Assert.Equal(1, remaining.Count());
        }
    }
}