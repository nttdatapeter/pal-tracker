using Microsoft.AspNetCore.Mvc;

namespace PalTracker
{
    [Route("/time-entries")]
    public class TimeEntryController : ControllerBase
    {
        private readonly ITimeEntryRepository _repository;
        private readonly IOperationCounter<TimeEntry> _operationCounter;

        public TimeEntryController(ITimeEntryRepository repository, IOperationCounter<TimeEntry> operationCounter)
        {
            _repository = repository;
            _operationCounter = operationCounter;
        }

        [HttpPost]
        public IActionResult Create([FromBody] TimeEntry timeEntry)
        {
            _operationCounter.Increment(TrackedOperation.Create);

            var createdTimeEntry = _repository.Create(timeEntry);

            return CreatedAtRoute("GetTimeEntry", new {id = createdTimeEntry.Id}, createdTimeEntry);
        }

        [HttpGet("{id}", Name = "GetTimeEntry")]
        public IActionResult Read(long id)
        {
            _operationCounter.Increment(TrackedOperation.Read);

            return _repository.Contains(id) ? (IActionResult) Ok(_repository.Find(id)) : NotFound();
        }

        [HttpGet]
        public IActionResult List()
        {
            _operationCounter.Increment(TrackedOperation.List);

            return Ok(_repository.List());
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TimeEntry timeEntry)
        {
            _operationCounter.Increment(TrackedOperation.Update);

            return _repository.Contains(id) ? (IActionResult) Ok(_repository.Update(id, timeEntry)) : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _operationCounter.Increment(TrackedOperation.Delete);

            if (!_repository.Contains(id))
            {
                return NotFound();
            }

            _repository.Delete(id);

            return NoContent();
        }
    }
}